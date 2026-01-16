using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VisualScripting;
using UnityEngine;

public class RotatingAndShoutingGuns : GunController
{
    [SerializeField] GameObject cartridgePrefab;
    [SerializeField] List<CartridgeSpawnSerializable> cartridgeSpawns;
    [SerializeField] RotatingAndShoutingGunsSettings gunSettings;
    [SerializeField] Crystals crystals;
    [SerializeField] float maxHeight;

    float _maxDegreesDelta = 2f;
    float _currentRechargeTime = 0f;
    float _currentAttackTime = 0f;

    int _countCartridges;
    int _currentCartridgeIndex = 0;

    public override int Level { get; protected set; }

    public RotatingAndShoutingGunSettingsSerializable Settings { get; private set; }
    public RotatingAndShoutingGunLevelSettingsSerializable LevelSettings 
    {
        get => (RotatingAndShoutingGunLevelSettingsSerializable)_levelSettings;
        set => _levelSettings = value; 
    }

    public override void Init(CollectMonsters _collection)
    {
        base.Init(_collection);
        SetSettings();
        Upgrade();

        _currentAttackTime = LevelSettings.attackInterval;
    }

    public void SetSettings()
    {
        foreach (RotatingAndShoutingGunSettingsSerializable _gunSettings in gunSettings.guns)
        {
            if (_gunSettings.type == Type)
            {
                Settings = _gunSettings;
                return;
            }
        }
    }

    protected void Rotate()
    {
        MonsterController _monster = Collection.Monsters[0];

        Vector3 _direction = _monster.transform.position - transform.position;
        _direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(_direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _maxDegreesDelta);
    }

    protected void Shout()
    {
        foreach (CartridgeSpawnSerializable _cartrides in cartridgeSpawns)
        {
            if (_cartrides.spawns.Count == _countCartridges)
            {
                GameObject _cartridgeObject = Instantiate
                (
                    cartridgePrefab, 
                    _cartrides.spawns[_currentCartridgeIndex].position, 
                    _cartrides.spawns[_currentCartridgeIndex].rotation
                );

                _cartridgeObject.transform.SetParent(Collection.transform);

                CartridgeController _cartridge = _cartridgeObject.GetComponent<CartridgeController>();
                _cartridge.Gun = this;

                Rigidbody _cartridgeRg = _cartridgeObject.GetComponent<Rigidbody>();
                _cartridgeRg.linearVelocity = GetVelocity(_cartridgeObject.transform.position, Collection.Monsters[0].transform.position, maxHeight);
            }
        }
    }

    Vector3 GetVelocity(Vector3 _start, Vector3 _end, float _maxHeight)
    {
        float _highestPoint = Mathf.Max(_start.y, _end.y) + _maxHeight;
        float _apexHeight = _highestPoint - _start.y;

        float _heightFromApex = _highestPoint - _end.y;

        float _timeToApex = Mathf.Sqrt(2 * _apexHeight / -Physics.gravity.y);
        float _timeFromApex = Mathf.Sqrt(2 * _heightFromApex / -Physics.gravity.y);
        float _totalTime = _timeToApex + _timeFromApex;

        Vector3 _velocity = Collection.Monsters[0].CurrentVelocity;
        _end += _velocity * _totalTime;

        Vector3 _horizontalDistance = new Vector3(_end.x - _start.x, 0, _end.z - _start.z);
        Vector3 _horizontalVelocity = _horizontalDistance / _totalTime;

        float _verticalVelocity = Mathf.Sqrt(2 * -Physics.gravity.y * _apexHeight);

        return _horizontalVelocity + Vector3.up * _verticalVelocity;
    }

    public override int Upgrade()
    {
        Level++;
        LevelSettings = (RotatingAndShoutingGunLevelSettingsSerializable)GetLevelSettings();

        if (crystals.crystals < LevelSettings.price)
        {
            Level--;
            LevelSettings = (RotatingAndShoutingGunLevelSettingsSerializable)GetLevelSettings();
            return -1;
        }

        meshFilter.mesh = LevelSettings.mesh;
        Collection.Radius = LevelSettings.radius;
        return LevelSettings.price;
    }

    public override bool IsCanUpgrade()
    {
        float _maxLevel = 0f;
        foreach (LevelSettings _levelUpgrade in Settings.levels)
        {
            _maxLevel = Mathf.Max(_maxLevel, _levelUpgrade.level);
        }

        return _maxLevel != Level;
    }

    protected override void GunHandle()
    {
        if (Collection == null || Collection.Monsters.Count == 0) return;

        Rotate();

        _currentRechargeTime += Time.deltaTime;
        if (LevelSettings.rechargeTime <= _currentRechargeTime)
        {
            _currentAttackTime += Time.deltaTime;
            _countCartridges = LevelSettings.countCartridges;

            if (LevelSettings.attackInterval <= _currentAttackTime)
            {
                Shout();
                _currentCartridgeIndex++;
                _currentAttackTime = 0f;
            }

            if (_currentCartridgeIndex >= _countCartridges)
            {
                _currentRechargeTime = 0f;
                _currentCartridgeIndex = 0;
                _currentAttackTime = LevelSettings.attackInterval;
            }
        }
    }

    public override GunLevelSettingsSerializable GetLevelSettings()
    {
        foreach (RotatingAndShoutingGunLevelSettingsSerializable _levelSettings in Settings.levels)
        {
            if (_levelSettings.level == Level)
                return _levelSettings;
        }

        return null;
    }

    [Serializable]
    class CartridgeSpawnSerializable
    {
        public List<Transform> spawns;
    }
}
