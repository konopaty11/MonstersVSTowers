using UnityEngine;

public class MagicCrystalController : GunController, IUpgradable
{
    [SerializeField] GunsSettings gunSettings;

    public int Level { get; private set; }

    GunSettingsSerializable _settings;

    public override void SetSettings()
    {
        foreach (GunSettingsSerializable _gunSettings in gunSettings.guns)
        {
            if (_gunSettings.type == Type)
            {
                _settings = _gunSettings;
                return;
            }
        }
    }

    public bool Upgrade()
    {
        throw new System.NotImplementedException();
    }

    void Attack()
    {
        
    }

    protected override void GunHandle()
    {
        if (Collection == null || Collection.Monsters.Count == 0) return;

        Attack();
    }

    public override GunLevelSettingsSerializable GetLevelSettings()
    {
        foreach (GunLevelSettingsSerializable _levelSettings in _settings.levels)
        {
            if (_levelSettings.level == Level)
                return _levelSettings;
        }

        return null;
    }
}
