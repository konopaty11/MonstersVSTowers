using TMPro;
using UnityEngine;

public class TowerWindowController : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] TextMeshProUGUI energyText;
    [SerializeField] TextMeshProUGUI countEnergyAddedCountText;
    [SerializeField] TextMeshProUGUI levelTowerText;
    [SerializeField] TextMeshProUGUI levelGunText;
    [SerializeField] TextMeshProUGUI priceTowerUpgradeText;
    [SerializeField] TextMeshProUGUI priceGunUpgradeText;

    [Header("Sections")]
    [SerializeField] CanvasGroup towerSection;
    [SerializeField] CanvasGroup gunSection;

    [Header("Tower section positions")]
    [SerializeField] Vector3 towerSectionPositionAlone;
    [SerializeField] Vector3 towerSectionPositionWithGunSection;

    [Header("Gun section positions")]
    [SerializeField] Vector3 gunSectionPositionAlone;
    [SerializeField] Vector3 gunSectionPositionWithTowerSection;

    [Header("Energy section positions")]
    [SerializeField] Vector3 energySectionPositionAlone;
    [SerializeField] Vector3 energySectionPositionNotAlone;

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
        {
            levelTowerText.text = _level.ToString();

            string _upgradePriceString;
            int _upgradePrice = _tower.GetUpgradePrice();
            if (_upgradePrice == -1)
                _upgradePriceString = "MAX";
            else
                _upgradePriceString = _upgradePrice.ToString(); 
            priceTowerUpgradeText.text = _upgradePriceString;
        }

        Debug.Log($"tower: {_level}");

        TowerSectionHandle();
        GunSectionHandle();
    }

    void UpdateEnergy(TowerController _tower, int _energy)
    {
        if (_tower == _currentTower)
        {
            energyText.text = _energy.ToString();
        }
    }

    void UpdateGunLevel(GunController _gun, int _level)
    {
        if (_gun == _currentTower.CurrentGun)
        {
            levelGunText.text = _level.ToString();

            string _upgradePriceString;
            int _upgradePrice = _gun.GetUpgradePrice();
            if (_upgradePrice == -1)
                _upgradePriceString = "MAX";
            else
                _upgradePriceString = _upgradePrice.ToString();
            priceGunUpgradeText.text = _upgradePriceString;
        }

        Debug.Log($"gun: {_level}");

        TowerSectionHandle();
        GunSectionHandle();
    }

    public void Setup(TowerController _tower, int _energy, int _countEnergyAddedCount, int _levelTower, int _priceTowerUpgrade, int _levelGun, int _priceGunUpgrade)
    {
        _currentTower = _tower;
        energyText.text = _energy.ToString();
        countEnergyAddedCountText.text = _countEnergyAddedCount.ToString();

        UpdateTowerLevel(_tower, _levelTower);

        if (_tower.CurrentGun != null)
            UpdateGunLevel(_tower.CurrentGun, _levelGun);
    }

    public void GunSectionHandle()
    {
        if (_currentTower.CurrentGun == null || !_currentTower.CurrentGun.IsCanUpgrade())
            gunSection.interactable = false;
        else
            gunSection.interactable = true;
        Debug.Log("gun handle");
        //towerSection.transform.localPosition = gunSection.interactable ?
        //    towerSectionPositionWithGunSection :
        //    towerSectionPositionAlone;
    }

    void TowerSectionHandle()
    {
        if (!_currentTower.IsCanUpgrade())
            towerSection.interactable = false;
        else
            towerSection.interactable = true;

        //gunSection.transform.localPosition = towerSection.interactable ?
        //    gunSectionPositionWithTowerSection :
        //    gunSectionPositionAlone;
    }
}
