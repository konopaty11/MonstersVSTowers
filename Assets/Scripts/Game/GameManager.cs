using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

/// <summary>
/// Îñíîâíîé ìåíåäæåð
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] ModeManager modeManager;
    [SerializeField] WavesSerializable waves;
    [SerializeField] MonstersSpawn monsterSpawn;
    [SerializeField] ResultWindowController looseWindow;
    [SerializeField] ResultWindowController winWindow;
    [SerializeField] GeneralSettings generalSettings;
    [SerializeField] VisibilityUIManager visibleUIManager;
    [SerializeField] LoadManager loadManager;
    [SerializeField] Crystals crystals;

    public static UnityAction<int> OnUpdateWave;
    public static UnityAction OnRestart;

    int _currentWaveIndex = -1;
    public int CurrentWave => _currentWaveIndex + 1;

    Coroutine _spawnCoroutine;

    string _towerTag = "Tower";

    InputSystem_Actions _inputSystem;

    LayerMask _layerMask;
    string _ignoreRaycastLayerName = "Ignore Raycast";

    bool _isLoose;

    string _winBackgroundID = "Win background";
    string _winID = "Win";
    string _looseBackgroundID = "Loose background";
    string _looseID = "Loose";

    public int CountKilledMonsters { get; set; }
    int _ñountCreatedGuns;
    int _ñountUpdatedGuns;
    int _ñountUpdatedTowers;
    float _timer;
    bool _timerActive;

    void Awake()
    {
        _inputSystem = new();
    }

    void OnEnable()
    {
        _inputSystem.Player.Attack.Enable();
        _inputSystem.Player.Attack.performed += ThrowRaycast; 
    }

    void OnDisable()
    {
        _inputSystem.Player.Attack.Disable();
        _inputSystem.Player.Attack.performed -= ThrowRaycast;
    }

    void Start()
    {
        Init();
    }

    private void Update()
    {
        TimerControl();
    }

    void Init()
    {
        _layerMask = ~LayerMask.GetMask(_ignoreRaycastLayerName);
    }

    void TimerControl()
    {
        if (_timerActive)
            _timer += Time.deltaTime;
        else
            _timer = 0f;
    }

    public void Play()
    {
        loadManager.LoadGame(NextWave);
    }

    public void AllMonstersDied()
    {
        if (_isLoose) return;

        if (CurrentWave == waves.waves.Count)
        {
            Win();
        }
        else if (_spawnCoroutine == null)
        {
            NextWave();
        }
    }

    public void NextWave()
    {
        _timerActive = true;

        _currentWaveIndex++;
        StartCoroutine(Spawn());
        OnUpdateWave?.Invoke(CurrentWave);
    }

    public void Restart()
    {
        ResetWave();
        CloseResultWindow();
        NextWave();
    }

    public void ToMenu()
    {
        ResetWave();
        CloseResultWindow();
        loadManager.LoadMenu();
    }

    void ResetWave()
    {
        OnRestart?.Invoke();
        _currentWaveIndex = -1;

        _ñountCreatedGuns = 0;
        _ñountUpdatedGuns = 0;
        _ñountUpdatedTowers = 0;
        _timerActive = false;
        _timer = 0f;
    }

    void CloseResultWindow()
    {
        if (_isLoose)
        {
            visibleUIManager.HideUI(_looseBackgroundID, ShowType.Fading);
            visibleUIManager.HideUI(_looseID, ShowType.Moving);
            looseWindow.SetActive(false);
        }
        else
        {
            visibleUIManager.HideUI(_winBackgroundID, ShowType.Fading);
            visibleUIManager.HideUI(_winID, ShowType.Moving);
            winWindow.SetActive(false);
        }
    }

    public void Win()
    {
        winWindow.SetActive(true);
        winWindow.UpdateUI(CurrentWave, CountKilledMonsters, _ñountCreatedGuns, _ñountUpdatedGuns, _ñountUpdatedTowers, _timer);

        visibleUIManager.ShowUI(_winBackgroundID, ShowType.Fading);
        visibleUIManager.ShowUI(_winID, ShowType.Moving);

    }

    public void Loose()
    {
        _isLoose = true;
        looseWindow.SetActive(true);
        looseWindow.UpdateUI(CurrentWave, CountKilledMonsters, _ñountCreatedGuns, _ñountUpdatedGuns, _ñountUpdatedTowers, _timer);

        visibleUIManager.ShowUI(_looseBackgroundID, ShowType.Fading);
        visibleUIManager.ShowUI(_looseID, ShowType.Moving);
    }

    void ThrowRaycast(InputAction.CallbackContext _context)
    {
        if (Touchscreen.current == null || modeManager.Mode == Modes.None) return;

        foreach (TouchControl _touch in Touchscreen.current.touches)
        {
            Vector2 _position = _touch.position.ReadValue();
            Ray _ray = mainCamera.ScreenPointToRay(_position);

            if (Physics.Raycast(_ray, out RaycastHit _hit, Mathf.Infinity, _layerMask) && _hit.collider.CompareTag(_towerTag))
            {
                TowerController _tower = _hit.collider.GetComponent<TowerController>();

                int _result = _tower.HandleTowerInteraction(modeManager.Mode);
                if (_result != -1)
                {
                    crystals.SubtractCrystals(_result);
                    UpdateParams();
                }

                modeManager.SetModeControl(Modes.None);
            }
        }
    }

    void UpdateParams()
    {
        switch (modeManager.Mode)
        {
            case >= Modes.CreatingCannon:
                _ñountCreatedGuns++;
                break;
            case Modes.UpgradingGuns:
                _ñountUpdatedGuns++;
                break;
            case Modes.UpgradingTowers:
                _ñountUpdatedTowers++;
                break;
        }
    }

    /// <summary>
    /// êîðóòèíà ñïàâíà ìîíñòðîâ
    /// </summary>
    /// <returns></returns>
    IEnumerator Spawn()
    {
        foreach (MonsterWaveSerializable _monster in waves.waves[_currentWaveIndex].monsters)
        {
            for (int i = 0; i < _monster.count; i++)
            {
                yield return new WaitForSeconds(Random.Range(generalSettings.minSpawnDelay, generalSettings.maxSpawnDelay));

                monsterSpawn.SpawnMonster(_monster.type);
            }
        }

        _spawnCoroutine = null;
    }

    public static Vector2 GetRealScreenSize()
    {
        if (Screen.orientation == ScreenOrientation.LandscapeLeft ||
            Screen.orientation == ScreenOrientation.LandscapeRight)
        {
            return new
                (
                Mathf.Max(Screen.height, Screen.width),
                Mathf.Min(Screen.height, Screen.width)
                );
        }

        return new(
                Mathf.Min(Screen.height, Screen.width),
                Mathf.Max(Screen.height, Screen.width)
                );
    }
}
