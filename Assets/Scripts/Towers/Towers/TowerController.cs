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

            case Modes.CreatingGuns:
                IsLock = !_isFree;
                break;
        }

        SetLock(IsLock);
    }

    void SetLock(bool _isLock)
    {
        meshRenderer.material = _isLock ? lockMaterial : unlockMaterial;
    }

    public void Upgrade()
    {
        
    }
}
