using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class GunController : MonoBehaviour, IUpgradable
{
    [SerializeField] GunType type;
    [SerializeField] protected MeshFilter meshFilter;

    public GunType Type => type;
    public CollectMonsters Collection { get; set; }
    public abstract int Level { get; protected set; }
    public bool Active { get; set; }

    protected GunLevelSettingsSerializable _levelSettings;

    void Update()
    {
        if (Active)
            GunHandle();
    }

    public abstract GunLevelSettingsSerializable GetLevelSettings();

    public virtual void Init(CollectMonsters _collection)
    {
        Collection = _collection;
    }

    protected abstract void GunHandle();

    public abstract int Upgrade();

    public abstract bool IsCanUpgrade();

    public abstract int CanAffordUpgrade();
}
