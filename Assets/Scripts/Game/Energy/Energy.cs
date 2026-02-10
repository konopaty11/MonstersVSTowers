using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Energy", menuName = "Scriptable Objects/Energy")]
public class Energy : ScriptableObject
{
    public int energy;
    public int maxEnergy = 100;
    public int energyForTowerCharge = 10;
    public int maxTowerEnergy = 30;
    public int energyConsuption = 1;
    public int energyConsuptionTime = 2;
    public List<WaveEnergySerializable> waveEnergy;

    public static UnityAction<int> OnEnergyChange;

    void Awake()
    {
        OnEnergyChange?.Invoke(energy);
    }

    void OnEnable()
    {
        Saves.OnDataLoaded += OnDataLoad;
    }

    void OnDisable()
    {
        Saves.OnDataLoaded -= OnDataLoad;
    }

    void OnDataLoad(SaveData _data)
    {
        SetEnergy(_data.energy);
    }

    public void SetEnergy(int _value)
    {
        energy = _value;
        OnEnergyChange?.Invoke(energy);
    }


}

[Serializable]
public class WaveEnergySerializable
{
    public int wave;
    public int energy;
}
