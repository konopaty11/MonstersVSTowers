using TMPro;
using UnityEngine;

public class TowerWindowController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI energyText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI priceTowerUpgradeText;

    TowerController _currentTower;

    void OnEnable()
    {
        TowerController.OnLevelUpgrade += UpdateLevel;
        TowerController.OnEnergyUpgrade += UpdateEnergy;
    }

    void OnDisable()
    {
        TowerController.OnLevelUpgrade -= UpdateLevel;
        TowerController.OnEnergyUpgrade -= UpdateEnergy;
    }

    void UpdateLevel(TowerController _tower, int _level)
    {
        if (_tower == _currentTower)
            levelText.text = _level.ToString();
    }

    void UpdateEnergy(TowerController _tower, int _energy)
    {
        if (_tower == _currentTower)
            energyText.text = _energy.ToString();
    }

    public void Init(TowerController _tower, int _energy, int _level, float _priceTowerUpgrade)
    {
        _currentTower = _tower;
        energyText.text = _energy.ToString();
        levelText.text = _level.ToString();
        priceTowerUpgradeText.text = _priceTowerUpgrade.ToString();
    }
}
