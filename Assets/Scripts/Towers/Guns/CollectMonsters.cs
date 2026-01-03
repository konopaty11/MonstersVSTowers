using System.Collections.Generic;
using UnityEngine;

public class CollectMonsters : MonoBehaviour
{
    string _monsterTag = "Monster";

    public CapsuleCollider Collider { get; private set; }

    public List<MonsterController> Monsters => _monsters;
    List<MonsterController> _monsters = new();

    void Start()
    {
        Collider = GetComponent<CapsuleCollider>();
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
}
