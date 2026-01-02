using UnityEngine;

public class TowerController : MonoBehaviour, IUpgradable
{
    [SerializeField] GunSpawn gunSpawn;
    [SerializeField] TowersUpgradeSerializable towerUpgrades;
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

    void LockControl(Modes _mode)
    {
        switch (_mode)
        {
            case Modes.UpgradingTowers:
                IsLock = towerUpgrades.towers.Count == Level;
                break;

            case Modes.UpgradingGuns:
                break;

            case >= Modes.CreatingCannon:
                IsLock = !_isFree;
                break;
        }

        SetLock(IsLock);
    }

    void SetLock(bool _isLock)
    {
        meshRenderer.material = _isLock ? lockMaterial : unlockMaterial;
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
