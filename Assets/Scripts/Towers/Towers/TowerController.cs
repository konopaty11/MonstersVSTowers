using UnityEngine;

public class TowerController : MonoBehaviour, IUpgradable
{
    [Header("Menu")]
    [SerializeField] bool isMenu;

    [Header("Gun")]
    [SerializeField] GunSpawn gunSpawn;
    [SerializeField] Transform cartridgesSpawn;
    [SerializeField] Transform deltaCrystals;

    [Header("Upgrades")]
    [SerializeField] TowersUpgradeSerializable towerUpgrades;
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] CollectMonsters collection;
    [SerializeField] VisibilityUIManager visibilityUIManager;
    [SerializeField] GameObject towerControlWindowObject;

    [Header("Lock\\Unlock")]
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Material lockMaterial;
    [SerializeField] Material unlockMaterial;

    [Header("Crystals")]
    [SerializeField] Crystals crystals;
    [SerializeField] Prices prices;

    [Header("Saves")]
    [SerializeField] RestoreTower restoreTower;

    public int Level { get; private set; } = 1;
    public bool IsLock { get; private set; }

    bool _isFree = true;

    GunController _currentGun;
    public GunController CurrentGun => _currentGun;

    float _refundRatioForDeleteGun = 0.75f;

    CrystalsAnimateManager _crystalsAnimate;

    string _towerWindowID = "Tower";

    void Start()
    {
        Init();
    }

    void OnEnable()
    {
        ModeManager.OnModeChange += LockControl;
        GameManager.OnRestart += ResetTower;
        GameManager.OnMenuTransition += ResetTower;
    }

    void OnDisable()
    {
        ModeManager.OnModeChange -= LockControl;
        GameManager.OnRestart -= ResetTower;
        GameManager.OnMenuTransition -= ResetTower;
    }

    void Init()
    {
        foreach (TowerLevelUpgradeSerializable _levelUpgrade in towerUpgrades.towers)
        {
            if (_levelUpgrade.level == Level)
            {
                collection.RadiusMultyplier = _levelUpgrade.rangeMultiplier;
            }
        }

        _crystalsAnimate = ServiceLocator.Get<CrystalsAnimateManager>();
    }

    public void OpenControlWindow()
    {
        towerControlWindowObject.SetActive(true);
        visibilityUIManager.ShowUI(_towerWindowID, ShowType.Fading);
    }

    // корутина для открывания закрывания окна

    public void CloseControlWindow()
    {
        visibilityUIManager.ShowUI(_towerWindowID, ShowType.Fading);
    }

    void LockControl(Modes _mode)
    {
        switch (_mode)
        {
            case Modes.None:
                SetLock();
                return;

            case Modes.UpgradingTowers:
                IsLock = !IsCanUpgrade();
                break;

            case Modes.UpgradingGuns:
                IsLock = !IsCanGunUpgrade();
                break;

            case Modes.DeletingGun:
                IsLock = _isFree;
                break;

            case >= Modes.CreatingCannon:
                IsLock = !_isFree;
                break;
        }

        SetLock(IsLock);
    }

    public bool IsCanUpgrade()
    {
        float _maxLevel = 0f;
        foreach (TowerLevelUpgradeSerializable _levelUpgrade in towerUpgrades.towers)
        {
            _maxLevel = Mathf.Max(_maxLevel, _levelUpgrade.level);
        }

        return _maxLevel != Level;
    }

    bool IsCanGunUpgrade()
    {
        if (_currentGun == null)
            return false;

        return _currentGun.IsCanUpgrade();
    }

    void SetLock(bool _isLock)
    {
        meshRenderer.material = _isLock ? lockMaterial : unlockMaterial;
    }

    void SetLock()
    {
        meshRenderer.material = lockMaterial;
    }

    public int HandleTowerInteraction(Modes _mode)
    {
        if (IsLock) return -1;

        int _result = _mode switch
        {
            Modes.UpgradingTowers => CanAffordUpgrade(),
            Modes.UpgradingGuns => CanAffordGunUpgrade(),
            Modes.DeletingGun => DeleteGun(),
            >= Modes.CreatingCannon => CanAffordGun(_mode),
            _ => -1
        };

        return _result;
    }

    void ResetTower()
    {
        if (isMenu) return;

        if (_currentGun != null)
            _currentGun.Collection.ResetMonsters();

        Level = 1;
        foreach (TowerLevelUpgradeSerializable _levelUpgrade in towerUpgrades.towers)
        {
            if (_levelUpgrade.level == Level)
            {
                meshFilter.mesh = _levelUpgrade.mesh;
                collection.RadiusMultyplier = _levelUpgrade.rangeMultiplier;
            }
        }

        if (_currentGun != null)
        {
            DestroyGun();
        }
    }

    public int DeleteGun()
    {
        int _crystalsRefund = (int)(GetGunCreatePrice((Modes)_currentGun.Type) * _refundRatioForDeleteGun);
        crystals.AddCrystals(_crystalsRefund);
        _crystalsAnimate.DeltaCrystalsPositionAnimate(deltaCrystals.position, _crystalsRefund);

        restoreTower.TowerSerializable.gunType = GunType.None;

        DestroyGun();

        return -1;
    }

    void DestroyGun()
    {
        _currentGun.Collection.ResetMonsters();

        Destroy(_currentGun.gameObject);
        _currentGun = null;
        _isFree = true;
    }

    public int CanAffordUpgrade()
    {
        if (crystals.crystals < prices.upgradeTower)
            return -1;

        int _price = Upgrade();
        restoreTower.TowerSerializable.level = Level;

        if(_price != -1)
            _crystalsAnimate.DeltaCrystalsPositionAnimate(deltaCrystals.position, -_price);

        return _price;
    }

    public int CanAffordGun(Modes _mode)
    {
        int _price = GetGunCreatePrice(_mode);
        if (crystals.crystals < _price)
            return -1;

        CreateGun((GunType)_mode);

        _crystalsAnimate.DeltaCrystalsPositionAnimate(deltaCrystals.position, -_price);

        restoreTower.TowerSerializable.gunType = (GunType)_mode;
        restoreTower.TowerSerializable.gunLevel = _currentGun.Level;

        return _price;
    }

    public int Upgrade()
    {
        Level++;

        foreach (TowerLevelUpgradeSerializable _levelUpgrade in towerUpgrades.towers)
        {
            if (_levelUpgrade.level == Level)
            {
                meshFilter.mesh = _levelUpgrade.mesh;
                collection.RadiusMultyplier = _levelUpgrade.rangeMultiplier;
                return prices.upgradeTower;
            }
        }

        return -1;
    }

    public void CreateGun(GunType _type)
    {
        _currentGun = gunSpawn.SpawnGun(_type);
        _isFree = false;
    }

    int CanAffordGunUpgrade()
    {
        int _price = _currentGun.CanAffordUpgrade();

        if (_price != -1)
        {
            restoreTower.TowerSerializable.gunLevel = _currentGun.Level;
            _crystalsAnimate.DeltaCrystalsPositionAnimate(deltaCrystals.position, -_price);
        }

        return _price;
    }

    int GetGunCreatePrice(Modes _mode)
    {
        return _mode switch
        {
            Modes.CreatingCannon => prices.createCannon,
            Modes.CreatingCrossbow => prices.createCrossbow,
            Modes.CreatingMagicCrystal => prices.createMagicCrystal,
            _ => -1
        };
    }
}
