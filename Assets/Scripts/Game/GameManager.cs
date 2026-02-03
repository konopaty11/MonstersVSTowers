using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] SoundManager soundManager;
    [SerializeField] MenuController menuController;
    [SerializeField] CastleController castleController;
    [SerializeField] List<RestoreTower> restoreTowers;
    [SerializeField] Saves saves;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] WaveResultWindowController waveResultWindowController;
    [SerializeField] CameraController cameraController;
    [SerializeField] GameObject towerControlWindowObject;
    [SerializeField] LayerMask mask;
    [SerializeField] TowerWindowController towerWindowController;
    [SerializeField] Prices prices;

    public static UnityAction<int, bool> OnUpdateWave;
    public static UnityAction OnRestart;
    public static UnityAction OnMenuTransition;

    int _currentWaveIndex = 0;
    public int CurrentWave => _currentWaveIndex + 1;

    Coroutine _spawnCoroutine;

    string _towerTag = "Tower"; 

    InputSystem_Actions _inputSystem;

    bool _isLoose;

    string _winBackgroundID = "Win background";
    string _winID = "Win";
    string _looseBackgroundID = "Loose background";
    string _looseID = "Loose";
    string _towerWindowID = "Tower";

    public int CountKilledMonsters { get; set; }
    int _ñountCreatedGuns;
    int _ñountUpdatedGuns;
    int _ñountUpdatedTowers;
    float _timer;

    public static bool TimerActive { get; set; }

    float _previusCatleHealth;
    float _previusTime;

    SaveData _data;

    List<MonsterSerializable> _monstersSerializable;

    TowerController _currentTower;

    void Awake()
    {
        _inputSystem = new();
    }

    void OnEnable()
    {
        _inputSystem.Enable();
        _inputSystem.UI.TouchDown.started += PressStarted;
        _inputSystem.UI.TouchDown.canceled += PressCancaled;
        _inputSystem.Player.Look.performed += DragHandle;
        Saves.OnDataLoaded += OnLoadData;
    }

    void OnDisable()
    {
        _inputSystem.Disable();
        _inputSystem.UI.Point.started -= PressStarted;
        Saves.OnDataLoaded -= OnLoadData;
    }

    void Update()
    {
        TimerControl();
    }

    void OnLoadData(SaveData _saveData)
    {
        _data = _saveData;
        menuController.LoadGameButtonActive(_saveData.isWaveSave);

        _monstersSerializable = _data.monsters;
    }

    public void LoadGame()
    {
        CountKilledMonsters = _data.countKilledMonsters;
        _ñountCreatedGuns = _data.countCreatedGuns;
        _ñountUpdatedGuns = _data.ñountUpdatedGuns;
        _ñountUpdatedTowers = _data.ñountUpdatedTowers;
        _timer = _data.timer;

        _currentWaveIndex = _data.wave - 1;
        castleController.LoadData(_data);
        foreach (RestoreTower _restoreTower in restoreTowers)
        {
            _restoreTower.LoadData(_data);
        }

        bool _allDied = true;
        foreach (MonsterSerializable _monsterSerializable in _monstersSerializable)
        {
            if (!_monsterSerializable.isDied)
            {
                _allDied = false;
                break;
            }
        }

        if (_allDied && _monstersSerializable.Count != 0)
        {
            _monstersSerializable = new();
            _currentWaveIndex++;
        }

        loadManager.LoadGame(StartGame);
        soundManager.ToMainMusic();
    }

    public void SaveAndQuitGame()
    {
        SaveGame();
        QuitGame();
    }

    public void QuitGame()
        => Application.Quit();

    void SaveGame()
    {
        saves.SetWave(CurrentWave, false);

        if (!StopSpawn())
        {
            _monstersSerializable = monsterSpawn.SaveMonsters();
            saves.SetMonsters(_monstersSerializable);
        }

        saves.SetResultParams(CountKilledMonsters, _ñountCreatedGuns, _ñountUpdatedGuns, _ñountUpdatedTowers, _timer);
    }

    void TimerControl()
    {
        if (TimerActive)
            _timer += Time.deltaTime;

        TimeSpan _span = TimeSpan.FromSeconds(_timer);
        timerText.text = _span.ToString(@"m\:ss");
    }

    public void NewGame()
    {
        _timer = 0f;
        _currentWaveIndex = 0;
        crystals.SetCrystals(crystals.baseCrystals);
        _monstersSerializable = new();
        saves.ResetData();
        
        loadManager.LoadGame(StartGame);
        soundManager.ToMainMusic();
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
            OpenWaveResultWindow();
        }
    }

    void OpenWaveResultWindow()
    {
        int _countStars = 1;
        if (castleController.CurrentHealth == _previusCatleHealth)
            _countStars++;
        if (_timer - _previusTime <= generalSettings.timeThresholdForStar)
            _countStars++;

        waveResultWindowController.OpenWindow(_timer, 0, 0, _countStars);
    }

    void StartGame()
    {
        TimerActive = true;

        SetPreviusParams();
        _spawnCoroutine = StartCoroutine(Spawn());
        OnUpdateWave?.Invoke(CurrentWave, true);
    }
    
    void SetPreviusParams()
    {
        _previusCatleHealth = castleController.CurrentHealth;
        _previusTime = _timer;
    }

    public void NextWave()
    {
        TimerActive = true;

        SetPreviusParams();

        _currentWaveIndex++;
        _monstersSerializable = new();
        saves.SetMonsters(_monstersSerializable);

        _previusCatleHealth = castleController.CurrentHealth;
        _spawnCoroutine = StartCoroutine(Spawn());
        OnUpdateWave?.Invoke(CurrentWave, false);
    }

    public void Restart()
    {
        ResetWave();
        CloseResultWindow();
        StartGame();
    }

    public void LoadSaveMenu()
    {
        loadManager.LoadMenu(ToMenu);
    }

    public void LoadResetMenu()
    {
        loadManager.LoadMenu(ResetWave);
        CloseResultWindow();
    }

    void ToMenu()
    {
        SaveGame();
        CloseResultWindow();
        soundManager.ToMenuMusic();
        TimerActive = false;

        menuController.LoadGameButtonActive(_data.isWaveSave);

        OnMenuTransition?.Invoke();
    }

    void ResetWave()
    {
        OnRestart?.Invoke();

        _isLoose = false;
        _currentWaveIndex = 0;

        _ñountCreatedGuns = 0;
        _ñountUpdatedGuns = 0;
        _ñountUpdatedTowers = 0;
        TimerActive = false;
        _timer = 0f;

        saves.SetWave(CurrentWave, false);
        _monstersSerializable = new();
        saves.SetMonsters(_monstersSerializable);
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
        if (_isLoose) return;

        _isLoose = true;
        looseWindow.SetActive(true);
        looseWindow.UpdateUI(CurrentWave, CountKilledMonsters, _ñountCreatedGuns, _ñountUpdatedGuns, _ñountUpdatedTowers, _timer);

        visibleUIManager.ShowUI(_looseBackgroundID, ShowType.Fading);
        visibleUIManager.ShowUI(_looseID, ShowType.Moving);
    }

    void DragStartedHandle(InputAction.CallbackContext context)
    {
        Debug.Log("drag start");

    }

    void DragHandle(InputAction.CallbackContext context)
    {
        Debug.Log("drag");

    }

    void PressStarted(InputAction.CallbackContext context)
    {
        Debug.Log("click start");


        if (Touchscreen.current == null) return;


        TouchControl _touch = Touchscreen.current.touches[0];
        Vector2 _position = _touch.position.ReadValue();
        ThrowRaycast(_position);
    }

    void PressCancaled(InputAction.CallbackContext context)
    {
        Debug.Log("press canceled");
    }

    public void ThrowRaycast(Vector2 _position)
    {
        Ray _ray = mainCamera.ScreenPointToRay(_position);

        Debug.DrawRay(_ray.origin, _ray.direction * 100f, Color.red, Mathf.Infinity);

        if (Physics.Raycast(_ray, out RaycastHit _hit, Mathf.Infinity, mask) && _hit.collider.CompareTag(_towerTag))
        {
            TowerController _tower = _hit.collider.GetComponent<TowerController>();

            if (modeManager.Mode == Modes.None)
            {
                cameraController.GoToTower(_tower.transform);
                _currentTower = _tower;
                OpenControlWindow();
            }
            else
            {
                TowerInteraction(_tower);
            }
        }
    }

    public void OpenControlWindow()
    {
        towerControlWindowObject.SetActive(true);
        visibleUIManager.ShowUI(_towerWindowID, ShowType.Fading);
        towerWindowController.Init(_currentTower, _currentTower.CurrentEnergy, _currentTower.Level, prices.upgradeTower);
    }

    public void CloseControlWindow()
    {
        StartCoroutine(CloseTowerWindowWithDelay());
    }

    public void AddTowerEnergy()
        => _currentTower.AddEnergy();

    public void UpgradeTower()
    {
        modeManager.SetModeControl(Modes.UpgradingTowers);
        TowerInteraction(_currentTower);
    }

    public void UpgradeGun()
    {
        modeManager.SetModeControl(Modes.UpgradingGuns);
        TowerInteraction(_currentTower);
    }

    IEnumerator CloseTowerWindowWithDelay()
    {
        float _delay = 0.3f;

        visibleUIManager.HideUI(_towerWindowID, ShowType.Fading);
        cameraController.GoToStartPosition();

        yield return new WaitForSeconds(_delay);
        towerControlWindowObject.SetActive(false);
    }

    public void TowerInteraction(TowerController _tower)
    {
        int _result = _tower.HandleTowerInteraction(modeManager.Mode);
        if (_result != -1)
        {
            crystals.SubtractCrystals(_result);
            UpdateParams();
        }

        modeManager.SetModeControl(Modes.None);
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

    bool StopSpawn()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            return true;
        }

        return false;
    }

    /// <summary>
    /// êîðóòèíà ñïàâíà ìîíñòðîâ
    /// </summary>
    /// <returns></returns>
    IEnumerator Spawn()
    {
        if (_monstersSerializable.Count == 0)
        {
            foreach (MonsterWaveSerializable _monster in waves.waves[_currentWaveIndex].monsters)
            {
                for (int i = 0; i < _monster.count; i++)
                {
                    yield return new WaitForSeconds(UnityEngine.Random.Range(generalSettings.minSpawnDelay, generalSettings.maxSpawnDelay));
                    
                    monsterSpawn.SpawnMonster(_monster.type);
                }
            }
        }
        else
        {
            foreach (MonsterSerializable _monster in _monstersSerializable)
            {
                monsterSpawn.SpawnMonster(_monster.monsterType, _monster);
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
