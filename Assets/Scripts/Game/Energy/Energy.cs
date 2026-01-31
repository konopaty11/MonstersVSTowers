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

    public static UnityAction<int> OnEnergyChange;

    public void SetEnergy(int _value)
    {
        energy = _value;
        OnEnergyChange?.Invoke(energy);
    }
}
