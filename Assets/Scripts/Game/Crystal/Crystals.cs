using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Crystals", menuName = "Scriptable Objects/Crystals")]
public class Crystals : ScriptableObject
{
    [SerializeField] Vector3 castleDeltaCrystalPosition;

    public static UnityAction<int> OnCountCrystalsChange;

    public int crystals;
    public List<MonsterPriceSerializable> monsterPrices;
    public List<WavePriceSerializable> wavePrices;

    CrystalsAnimateManager _crystalsAnimate;

    void OnEnable()
    {
        MonsterController.OnMonsterDied += AddCrystalsForMonster;
        GameManager.OnUpdateWave += AddCrystalsForPassedWave;
        Saves.OnDataLoaded += OnLoadData;
    }

    void OnDisable()
    {
        MonsterController.OnMonsterDied -= AddCrystalsForMonster;
        GameManager.OnUpdateWave -= AddCrystalsForPassedWave;
        Saves.OnDataLoaded -= OnLoadData;
    }

    void OnLoadData(SaveData _saveData)
    {
        crystals = _saveData.crystals;
    }

    public void SubtractCrystals(int _value)
    {
        crystals -= _value;
        OnCountCrystalsChange?.Invoke(crystals);
    }

    public void AddCrystals(int _value)
    {
        crystals += _value;
        OnCountCrystalsChange?.Invoke(crystals);
    }

    public void AddCrystalsForMonster(MonsterController _monster, bool _isMenu)
    {
        if (_isMenu) return;

        foreach (MonsterPriceSerializable _monsterPrice in monsterPrices)
        {
            if (_monsterPrice.type == _monster.Type)
            {
                AddCrystals(_monsterPrice.price);

                if (_crystalsAnimate == null)
                    _crystalsAnimate = ServiceLocator.Get<CrystalsAnimateManager>();

                _crystalsAnimate.DeltaCrystalsPositionAnimate(_monster.transform.position, _monsterPrice.price);
            }
        }
    }

    public void AddCrystalsForPassedWave(int _wave)
    {
        foreach (WavePriceSerializable _wavePrice in wavePrices)
        {
            if (_wavePrice.wave == _wave)
            {
                AddCrystals(_wavePrice.price);

                if (_crystalsAnimate == null)
                    _crystalsAnimate = ServiceLocator.Get<CrystalsAnimateManager>();

                _crystalsAnimate.DeltaCrystalsPositionAnimate(castleDeltaCrystalPosition, _wavePrice.price);
            }
        }
    }
}

[Serializable]
public class MonsterPriceSerializable
{
    public MonsterType type;
    public int price;
}

[Serializable]
public class WavePriceSerializable
{
    public int wave;
    public int price;
}
