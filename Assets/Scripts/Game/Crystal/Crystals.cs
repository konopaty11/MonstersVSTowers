using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Crystals", menuName = "Scriptable Objects/Crystals")]
public class Crystals : ScriptableObject
{
    public static UnityAction<int> OnCountCrystalsChange;

    public int crystals;

    public void SubtractCrystals(int _value)
    {
        crystals -= _value;
        OnCountCrystalsChange?.Invoke(crystals);
    }
}
