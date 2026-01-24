using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class Saves : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] Crystals crystals;
    [SerializeField] GeneralSettings generalSettings;
    [SerializeField] List<RestoreTower> towers;
    [SerializeField] SoundManager soundManager;

    public static UnityAction<SaveData> OnDataLoaded;

    SaveData _data;

    string _fileName = "UserData.json";
    string _path;

    FileInfo _file;

    List<TowerSerializable> _towersSerializable = new();

    void OnEnable()
    {
        GameManager.OnUpdateWave += SetWave;
        Crystals.OnCountCrystalsChange += SetCrystals;
    }

    private void OnDisable()
    {
        GameManager.OnUpdateWave -= SetWave;
        Crystals.OnCountCrystalsChange -= SetCrystals;
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        _path = Path.Combine(Application.persistentDataPath, _fileName);
        if (!Directory.Exists(Application.persistentDataPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath);
            return;
        }

        _file = new(_path);
        if (!_file.Exists)
        {
            _file.Create().Dispose();

            SetTowersSerializable();
            _data = new(crystals.crystals, soundManager.Volume, generalSettings.castleHealth, gameManager.CurrentWave, _towersSerializable);
            OnDataLoaded?.Invoke(_data);

            SaveData();
            return;
        }

        LoadGeneralData();
    }

    void SetTowersSerializable()
    {
        foreach (RestoreTower _tower in towers)
        {
            _towersSerializable.Add(_tower.TowerSerializable);
        }
    }

    public void LoadGeneralData()
    {
        string _jsonData = File.ReadAllText(_path);
        _data = JsonUtility.FromJson<SaveData>(_jsonData);
        OnDataLoaded?.Invoke(_data);
    }

    public void SaveData()
    {
        string _jsonData = JsonUtility.ToJson(_data, true);
        File.WriteAllText(_path, _jsonData);
    }

    public void SetCrystals(int _crystals)
    {
        _data.crystals = _crystals;
        SaveData();
    }

    public void SetWave(int _wave, bool _isStartWave)
    {
        _data.wave = _wave;
        _data.isWaveSave = true;
        SaveData();
    }

    public void SetVolume(float _volume)
    {
        _data.volume = _volume;
        SaveData();
    }

    public void SetCastleHealth(float _health)
    {
        _data.castleHealth = _health;
    }

    public void SetMonsters(List<MonsterSerializable> _monsters)
    {
        _data.monsters = _monsters;
        _data.isWaveSave = true;
        SaveData();
    }
}
