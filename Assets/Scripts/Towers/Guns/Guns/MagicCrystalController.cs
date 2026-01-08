using UnityEngine;

public class MagicCrystalController : GunController
{
    [SerializeField] MagicCrystalSettings settings;
    [SerializeField] MeshFilter stonesMeshFilter;

    public override int Level { get; protected set; }

    public MagicCrystalLevelSettingsSerializable LevelSettings
    {
        get => (MagicCrystalLevelSettingsSerializable)_levelSettings;
        set => _levelSettings = value;
    }

    float _currentTime;

    public override bool Upgrade()
    {
        Level++;
        LevelSettings = (MagicCrystalLevelSettingsSerializable)GetLevelSettings();

        stonesMeshFilter.mesh = LevelSettings.stonesMesh;
        meshFilter.mesh = LevelSettings.mesh;
        Collection.Radius = LevelSettings.radius;

        return true;
    }

    public override bool IsCanUpgrade()
    {
        float _maxLevel = 0f;
        foreach (LevelSettings _levelUpgrade in settings.levels)
        {
            _maxLevel = Mathf.Max(_maxLevel, _levelUpgrade.level);
        }

        return _maxLevel != Level;
    }

    public override void Init(CollectMonsters _collection)
    {
        base.Init(_collection);
        Upgrade();

        _currentTime = LevelSettings.rechargeTime;
    }

    void Attack()
    {
        float _slowSpeedCoefficient = LevelSettings.slowSpeedCoefficient;

        MonsterController[] _monsters = new MonsterController[Collection.Monsters.Count];
        Collection.Monsters.CopyTo(_monsters);

        foreach (MonsterController _monster in _monsters)
        {
            _monster.LastAttackedGun = this;
            _monster.SubstractHealth(LevelSettings.damage);

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
        if (LevelSettings.rechargeTime <= _currentTime)
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
