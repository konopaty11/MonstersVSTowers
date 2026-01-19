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

    SaveData _saveData;

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
            _saveData = new(crystals.crystals, soundManager.Volume, generalSettings.castleHealth, gameManager.CurrentWave, _towersSerializable);
            OnDataLoaded?.Invoke(_saveData);

            SaveData();
            return;
        }

        LoadData();
    }

    void SetTowersSerializable()
    {
        foreach (RestoreTower _tower in towers)
        {
            TowerSerializable _towerSerializable = new();
            _towerSerializable.id = _tower.ID;
            _towersSerializable.Add(_towerSerializable);
        }
    }

    public void LoadData()
    {
        string _data = File.ReadAllText(_path);
        _saveData = JsonUtility.FromJson<SaveData>(_data);
        OnDataLoaded?.Invoke(_saveData);
    }

    public void SaveData()
    {
        string _data = JsonUtility.ToJson(_saveData, true);
        File.WriteAllText(_path, _data);
    }

    public void SetCrystals(int _crystals)
    {
        _saveData.crystals = _crystals;
        SaveData();
    }

    public void SetWave(int _wave)
    {
        _saveData.wave = _wave;
        SaveData();
    }

    public void SetVolume(float _volume)
    {
        _saveData.volume = _volume;
        SaveData();
    }

    public void SetCastleHealth(float _health)
    {
        _saveData.castleHealth = _health;
    }
}
