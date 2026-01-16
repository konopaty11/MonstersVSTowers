using UnityEngine;

public class TowerController : MonoBehaviour, IUpgradable
{
    [Header("Gun")]
    [SerializeField] GunSpawn gunSpawn;
    [SerializeField] Transform cartridgesSpawn;

    [Header("Upgrades")]
    [SerializeField] TowersUpgradeSerializable towerUpgrades;
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] CollectMonsters collection;

    [Header("Lock\\Unlock")]
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Material lockMaterial;
    [SerializeField] Material unlockMaterial;

    [Header("Configurations")]
    [SerializeField] Crystals crystals;
    [SerializeField] Prices prices;

    public int Level { get; private set; }
    public bool IsLock { get; private set; }

    bool _isFree = true;

    GunController _currentGun;

    void Awake()
    {
        Upgrade();
    }

    void OnEnable()
    {
        ModeManager.OnModeChange += LockControl;
        GameManager.OnRestart += DestroyGun;
    }

    void OnDisable()
    {
        ModeManager.OnModeChange -= LockControl;
        GameManager.OnRestart -= DestroyGun;
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
            Modes.UpgradingTowers => Upgrade(),
            Modes.UpgradingGuns => GunUpgrade(),
            >= Modes.CreatingCannon => CreateGun(_mode),
            _ => -1
        };

        //LockControl(_mode);

        return _result;
    }

    void DestroyGun()
    {
        if (_currentGun != null)
        {
            Destroy(_currentGun.gameObject);
            _currentGun = null;
            _isFree = true;
        }
    }

    public int Upgrade()
    {
        if (crystals.crystals < prices.upgradeTower)
            return -1;

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

    int CreateGun(Modes _mode)
    {
        int _price = GetGunCreatePrice(_mode); 
        if (crystals.crystals < _price)
            return -1;

        _currentGun = gunSpawn.SpawnGun((GunType) _mode);
        _isFree = false;

        return _price;
    }

    public void CreateMenuGun(Modes _mode)
    {
        _currentGun = gunSpawn.SpawnGun((GunType)_mode);
    }

    int GunUpgrade()
    {
        return _currentGun.Upgrade();
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
