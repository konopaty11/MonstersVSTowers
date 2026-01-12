using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectMonsters : MonoBehaviour
{
    public static UnityAction<MonsterController> OnRemoveMonster;

    string _monsterTag = "Monster";

    float _radiusMultyplier;
    public float RadiusMultyplier 
    {
        get => _radiusMultyplier; 
        set
        {
            _radiusMultyplier = value;
            SetColliderRadius(Radius);
        } 
    }

    float _radius;
    public float Radius
    {
        get => _radius;
        set
        {
            _radius = value;
            SetColliderRadius(Radius);
        }
    }

    CapsuleCollider _collider;

    List<MonsterController> _monsters = new();
    public List<MonsterController> Monsters => _monsters;

    void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
    }

    void OnEnable()
    {
        OnRemoveMonster += RemoveMonster;
    }

    void OnDisable()
    {
        OnRemoveMonster -= RemoveMonster;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(_monsterTag)) return;

        _monsters.Add(other.GetComponent<MonsterController>());
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(_monsterTag)) return;

        MonsterController _monster = other.GetComponent<MonsterController>();
        if (!_monsters.Contains(_monster))
            _monsters.Add(_monster);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(_monsterTag)) return;

        _monsters.Remove(other.GetComponent<MonsterController>());
    }

    public void HandleRemoveMonster(MonsterController _monster)
    {
        RemoveMonster(_monster);
        OnRemoveMonster?.Invoke(_monster);
    }

    void RemoveMonster(MonsterController _monster)
    {
        _monsters.Remove(_monster);
    }

    void SetColliderRadius(float _radius)
    {
        _collider.radius = _radius * RadiusMultyplier;
    }
}
