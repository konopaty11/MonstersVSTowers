using UnityEngine;

public class MagicCrystalController : GunController, IUpgradable
{
    [SerializeField] GunsSettings gunSettings;

    public int Level { get; private set; }

    GunSettingsSerializable _settings;

    float _currentTime;

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
        foreach (MonsterController _monster in Collection.Monsters)
        {
            _monster.SubstractHealth(GetLevelSettings().damage);
        }
    }

    protected override void GunHandle()
    {
        if (Collection == null || Collection.Monsters.Count == 0) return;

        _currentTime += Time.deltaTime;
        if (GetLevelSettings().attackInterval <= _currentTime)
        {
            Attack();
            _currentTime = 0f;
        }
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
