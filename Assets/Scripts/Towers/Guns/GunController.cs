using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class GunController : MonoBehaviour, IUpgradable
{
    [SerializeField] GunType type;
    [SerializeField] protected MeshFilter meshFilter;

    public static UnityAction<GunController, int> OnUpgrade;

    public GunType Type => type;
    public CollectMonsters Collection { get; set; }
    public abstract int Level { get; protected set; }
    public bool Active { get; set; } = true;

    protected GunLevelSettingsSerializable _levelSettings;
    protected int _maxLevel;

    void Update()
    {
        if (Active)
            GunHandle();
    }

    public abstract GunLevelSettingsSerializable GetLevelSettings(int _level);

    public virtual void Init(CollectMonsters _collection)
    {
        Collection = _collection;
    }

    public int GetUpgradePrice()
    {
        if (IsCanUpgrade())
            return GetLevelSettings(Level + 1).price;

        return -1;
    }

    protected abstract void GunHandle();

    public virtual int Upgrade()
    {
        OnUpgrade?.Invoke(this, Level);
        return -1;
    }

    public abstract bool IsCanUpgrade();

    public abstract int CanAffordUpgrade();
}
