using UnityEngine;
using UnityEngine.UI;

public class EnergyUIManager : MonoBehaviour
{
    [SerializeField] Slider energySlider;
    [SerializeField] Energy energy;

    void OnEnable()
    {
        Energy.OnEnergyChange += UpdateEnergyUI;
    }

    void OnDisable()
    {
        Energy.OnEnergyChange -= UpdateEnergyUI;
    }

    void UpdateEnergyUI(int _energy)
    {
        energySlider.value = (float)_energy / energy.maxEnergy;
        Debug.Log(_energy);
    }
}
