using System.Collections.Generic;
using UnityEngine;

public class MagicCrystalController : GunController, IUpgradable
{
    [SerializeField] MagicCrystalSettings settings;

    public int Level { get; private set; } = 1;

    float _currentTime;

    public bool Upgrade()
    {
        throw new System.NotImplementedException();
    }

    public override void Init(CollectMonsters _collection)
    {
        base.Init(_collection);

        Collection.Radius = GetLevelSettings().radius;
        _currentTime = GetLevelSettings().attackInterval;
    }

    void Attack()
    {
        float _slowSpeedCoefficient = ((MagicCrystalLevelSettingsSerializable) GetLevelSettings()).slowSpeedCoefficient;

        MonsterController[] _monsters = new MonsterController[Collection.Monsters.Count];
        Collection.Monsters.CopyTo(_monsters);

        foreach (MonsterController _monster in _monsters)
        {
            _monster.LastAttackedGun = this;
            _monster.SubstractHealth(GetLevelSettings().damage);

            MonsterEffectController _monsterEffectController = _monster.GetComponent<MonsterEffectController>();
            SlowEffect _slowEffect = (SlowEffect) _monsterEffectController.GetEffect(EffectType.Slow);

            _slowEffect.SlowSpeedCoefficient = _slowSpeedCoefficient;
            _slowEffect.StartEffect();
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
        foreach (MagicCrystalLevelSettingsSerializable _levelSettings in settings.levels)
        {
            if (_levelSettings.level == Level)
                return _levelSettings;
        }

        return null;
    }
}
