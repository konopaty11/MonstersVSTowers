using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VisualScripting;
using UnityEngine;

public class RotatingAndShoutingGuns : GunController, IUpgradable
{
    [SerializeField] GameObject cartridgePrefab;
    [SerializeField] List<Transform> spawnsAmmunition;
    [SerializeField] RotatingAndShoutingGunsSettings gunSettings;
    [SerializeField] float maxHeight;

    float _maxDegreesDelta = 2f;
    float _currentTime = 0f;

    public int Level { get; private set; }

    RotatingAndShoutingGunSettingsSerializable _settings;

    public override void SetSettings()
    {
        foreach (RotatingAndShoutingGunSettingsSerializable _gunSettings in gunSettings.guns)
        {
            if (_gunSettings.type == Type)
            {
                _settings = _gunSettings;
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
        int _countGuns = ((RotatingAndShoutingGunLevelSettingsSerializable) GetLevelSettings()).countGuns;

        for (int i = 0; i < _countGuns; i++)
        {
            GameObject _cartridge = Instantiate(cartridgePrefab, spawnsAmmunition[i]);
            Rigidbody _cartridgeRg = _cartridge.GetComponent<Rigidbody>();

            _cartridgeRg.linearVelocity = GetVelocity(_cartridge.transform.position, Collection.Monsters[0].transform.position, maxHeight);
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

        Vector3 _horizontalDistance = new Vector3(_end.x - _start.x, 0, _end.z - _start.z);
        Vector3 _horizontalVelocity = _horizontalDistance / _totalTime;

        float _verticalVelocity = Mathf.Sqrt(2 * -Physics.gravity.y * _apexHeight);

        return _horizontalVelocity + Vector3.up * _verticalVelocity;
    }

    public bool Upgrade()
    {
        return true;
    }

    protected override void GunHandle()
    {
        if (Collection == null || Collection.Monsters.Count == 0) return;

        Rotate();

        _currentTime += Time.deltaTime;
        if (GetLevelSettings().attackInterval <= _currentTime)
        {
            Shout();
            _currentTime = 0f;
        }
    }



    public override void Init(CollectMonsters _collection)
    {
        base.Init(_collection);
        Collection.Collider.radius = GetLevelSettings().radius;
    }

    public override GunLevelSettingsSerializable GetLevelSettings()
    {
        foreach (RotatingAndShoutingGunLevelSettingsSerializable _levelSettings in _settings.levels)
        {
            if (_levelSettings.level == Level)
                return _levelSettings;
        }

        return null;
    }
}
