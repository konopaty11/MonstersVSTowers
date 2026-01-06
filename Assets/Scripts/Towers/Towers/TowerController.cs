using UnityEngine;

public class TowerController : MonoBehaviour, IUpgradable
{
    [Header("Gun spawn")]
    [SerializeField] GunSpawn gunSpawn;

    [Header("Upgrades")]
    [SerializeField] TowersUpgradeSerializable towerUpgrades;
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] CollectMonsters collection;

    [Header("Lock\\Unlock")]
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Material lockMaterial;
    [SerializeField] Material unlockMaterial;

    public int Level { get; private set; }
    public bool IsLock { get; private set; }

    bool _isFree = true;

    GunController _currentGun;

    void OnEnable()
    {
        ModeManager.OnModeChange += LockControl;
    }

    void OnDisable()
    {
        ModeManager.OnModeChange -= LockControl;
    }

    void Start()
    {
        Upgrade();
    }

    void LockControl(Modes _mode)
    {
        switch (_mode)
        {
            case Modes.None:
                SetLock();
                return;

            case Modes.UpgradingTowers:
                IsLock = IsCanUpgrade();
                break;

            case Modes.UpgradingGuns:
                break;

            case >= Modes.CreatingCannon:
                IsLock = !_isFree;
                break;
        }

        SetLock(IsLock);
    }

    bool IsCanUpgrade()
    {
        float _maxLevel = 0f;
        foreach (TowerLevelUpgradeSerializable _levelUpgrade in towerUpgrades.towers)
        {
            _maxLevel = Mathf.Max(_maxLevel, _levelUpgrade.level);
        }

        return _maxLevel == Level;
    }

    void SetLock(bool _isLock)
    {
        meshRenderer.material = _isLock ? lockMaterial : unlockMaterial;
    }

    void SetLock()
    {
        meshRenderer.material = lockMaterial;
    }

    public bool HandleTowerInteraction(Modes _mode)
    {
        if (IsLock) return false;

        bool _result = _mode switch
        {
            Modes.UpgradingTowers => Upgrade(),
            Modes.UpgradingGuns => GunUpgrade(),
            >= Modes.CreatingCannon => CreateGun(_mode),
            _ => false
        };

        LockControl(_mode);

        return _result;
    }

    public bool Upgrade()
    {
        Level++;

        foreach (TowerLevelUpgradeSerializable _levelUpgrade in towerUpgrades.towers)
        {
            if (_levelUpgrade.level == Level)
            {
                meshFilter.mesh = _levelUpgrade.mesh;
                collection.RadiusMultyplier = _levelUpgrade.rangeMultiplier;

                return true;
            }
        }

        return false;
    }

    bool CreateGun(Modes _mode)
    {
        _currentGun = gunSpawn.SpawnGun((GunType) _mode);
        _isFree = false;

        return true;
    }

    bool GunUpgrade()
    {
        return false;
    }
}
