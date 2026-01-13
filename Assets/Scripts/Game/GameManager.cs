using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

/// <summary>
/// Основной менеджер
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] ModeManager modeManager;
    [SerializeField] WavesSerializable waves;
    [SerializeField] MonstersSpawn monsterSpawn;
    [SerializeField] GameObject looseCanvas;
    [SerializeField] GameObject winCanvas;
    [SerializeField] GeneralSettings generalSettings;
    [SerializeField] VisibilityUIManager visibleUIManager;

    public static UnityAction<int> OnUpdateWave;

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

    void Init()
    {
        _layerMask = ~LayerMask.GetMask(_ignoreRaycastLayerName);
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
        _currentWaveIndex++;
        StartCoroutine(Spawn());
        OnUpdateWave?.Invoke(CurrentWave);
    }

    public void Win()
    {
        winCanvas.SetActive(true);
        visibleUIManager.ShowUI(_winBackgroundID, ShowType.Fading);
        visibleUIManager.ShowUI(_winID, ShowType.Moving);
    }

    public void Loose()
    {
        _isLoose = true;
        looseCanvas.SetActive(true);
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
                _tower.HandleTowerInteraction(modeManager.Mode);
                modeManager.SetModeControl(Modes.None);
            }
        }
    }

    /// <summary>
    /// корутина спавна монстров
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
}
