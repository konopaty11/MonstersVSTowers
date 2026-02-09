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
    [SerializeField] GameObject towerSection;
    [SerializeField] GameObject gunSection;
    [SerializeField] GameObject energySection;

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
            levelTowerText.text = _level.ToString();

        TowerSectionHandle();
        EnergySectionHandle();
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
            levelGunText.text = _level.ToString();

        GunSectionHandle();
        EnergySectionHandle();
    }

    public void Setup(TowerController _tower, int _energy, int _countEnergyAddedCount, int _levelTower, int _priceTowerUpgrade, int _levelGun, int _priceGunUpgrade)
    {
        _currentTower = _tower;
        energyText.text = _energy.ToString();
        countEnergyAddedCountText.text = _countEnergyAddedCount.ToString();

        levelTowerText.text = _levelTower.ToString();
        priceTowerUpgradeText.text = _priceTowerUpgrade.ToString();

        levelGunText.text = _levelGun.ToString();
        priceGunUpgradeText.text = _priceGunUpgrade.ToString();

        GunSectionHandle();
        TowerSectionHandle();
        EnergySectionHandle();
    }

    void GunSectionHandle()
    {
        if (_currentTower.CurrentGun == null || !_currentTower.CurrentGun.IsCanUpgrade())
            gunSection.SetActive(false);
        else
            gunSection.SetActive(true);

        towerSection.transform.localPosition = gunSection.activeSelf ?
            towerSectionPositionWithGunSection :
            towerSectionPositionAlone;
    }

    void TowerSectionHandle()
    {
        if (!_currentTower.IsCanUpgrade())
            towerSection.SetActive(false);

        gunSection.transform.localPosition = towerSection.activeSelf ?
            gunSectionPositionWithTowerSection :
            gunSectionPositionAlone;
    }

    void EnergySectionHandle()
    {
        if (!gunSection.activeSelf && !towerSection.activeSelf)
            energySection.transform.localPosition = energySectionPositionAlone;
        else
            energySection.transform.localPosition = energySectionPositionNotAlone;
    }
}
