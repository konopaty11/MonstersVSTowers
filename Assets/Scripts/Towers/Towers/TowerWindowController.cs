using TMPro;
using UnityEngine;

public class TowerWindowController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI energyText;
    [SerializeField] TextMeshProUGUI countEnergyAddedCount;
    [SerializeField] TextMeshProUGUI levelTowerText;
    [SerializeField] TextMeshProUGUI levelGunText;
    [SerializeField] TextMeshProUGUI priceTowerUpgradeText;
    [SerializeField] TextMeshProUGUI priceGunUpgradeText;
    [SerializeField] GameObject towerSection;
    [SerializeField] GameObject gunSection;
    [SerializeField] Vector3 towerSectionPositionAlone;
    [SerializeField] Vector3 towerSectionPositionWithGunSection;

    TowerController _currentTower;

    void OnEnable()
    {
        TowerController.OnLevelUpgrade += UpdateTowerLevel;
        TowerController.OnEnergyUpgrade += UpdateEnergy;
        GunController.OnUpgrade += UpdateGunLevel;
    }

    void OnDisable()
    {
        TowerController.OnLevelUpgrade -= UpdateTowerLevel;
        TowerController.OnEnergyUpgrade -= UpdateEnergy;
        GunController.OnUpgrade -= UpdateGunLevel;
    }

    void UpdateTowerLevel(TowerController _tower, int _level)
    {
        if (_tower == _currentTower)
            levelTowerText.text = _level.ToString();
    }

    void UpdateEnergy(TowerController _tower, int _energy)
    {
        if (_tower == _currentTower)
            energyText.text = _energy.ToString();
    }

    void UpdateGunLevel(GunController _gun, int _level)
    {
        if (_gun == _currentTower.CurrentGun)
            levelGunText.text = _level.ToString();
    }

    public void Setup(TowerController _tower, int _energy, int _level, float _priceTowerUpgrade)
    {
        _currentTower = _tower;
        energyText.text = _energy.ToString();
        levelTowerText.text = _level.ToString();
        priceTowerUpgradeText.text = _priceTowerUpgrade.ToString();

        if (_currentTower.CurrentGun == null)
            RemoveGunSection();
        else
            AddGunSection();
    }

    void AddGunSection()
    {
        gunSection.SetActive(true);
        towerSection.transform.position = towerSectionPositionWithGunSection;
    }

    void RemoveGunSection()
    {
        gunSection.SetActive(false);
        towerSection.transform.position = towerSectionPositionAlone;
    }
}
