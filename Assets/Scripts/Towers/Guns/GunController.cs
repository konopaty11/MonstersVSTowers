using System.Collections.Generic;
using UnityEngine;

public abstract class GunController : MonoBehaviour
{
    [SerializeField] GunType type;

    public GunType Type => type;
    public CollectMonsters Collection { get; set; }

    void Update()
    {
        GunHandle();
    }

    public abstract void SetSettings();

    public abstract GunLevelSettingsSerializable GetLevelSettings();

    public virtual void Init(CollectMonsters _collection)
    {
        Collection = _collection;

        SetSettings();
    }

    protected abstract void GunHandle();
}
