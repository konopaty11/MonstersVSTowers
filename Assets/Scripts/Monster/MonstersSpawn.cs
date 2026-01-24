using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// логика спавна монстров
/// </summary>
public class MonstersSpawn : MonoBehaviour
{
    [SerializeField] Transform monsterParent;
    [SerializeField] List<GameObject> monsterPrefabs;
    [SerializeField] SplineContainer spline;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject healthBarPrefab;
    [SerializeField] Transform healthBarParent;

    List<GameObject> _monsters = new();

    void OnEnable()
    {
        MonsterController.OnMonsterDestroy += CheckMonsters;
        GameManager.OnRestart += DestroyMonsters;
        GameManager.OnMenuTransition += DestroyMonsters;
    }

    void OnDisable()
    {
        MonsterController.OnMonsterDestroy -= CheckMonsters;
        GameManager.OnRestart -= DestroyMonsters;
        GameManager.OnMenuTransition -= DestroyMonsters;
    }

    /// <summary>
    /// спавн монстров
    /// </summary>
    /// <param name="_type"> тип монстра </param>
    public void SpawnMonster(MonsterType _type, MonsterSerializable _monsterSerializable = null)
    {
        foreach (GameObject _monsterPrefab in monsterPrefabs)
        {
            MonsterController _controller = _monsterPrefab.GetComponent<MonsterController>();
            if (_controller.Type == _type)
            {
                GameObject _monster = Instantiate(_monsterPrefab, monsterParent);
                _monsters.Add(_monster);

                GameObject _healthBarObject = Instantiate(healthBarPrefab, healthBarParent);
                HealthBarController _healthBar = _healthBarObject.GetComponent<HealthBarController>();
                _healthBar.Init(_monster.transform);

                MonsterController _monsterController = _monster.GetComponent<MonsterController>();
                _monsterController.InitMonster(spline, _healthBar, _monsterSerializable);
            }
        }    
    }

    public List<MonsterSerializable> SaveMonsters()
    {
        List<MonsterSerializable> _monstersSerializable = new List<MonsterSerializable>();

        foreach (GameObject _monsterObject in _monsters)
        {
            MonsterController _monster = _monsterObject.GetComponent<MonsterController>();
            _monstersSerializable.Add(_monster.SaveMonsterSerializable());
        }

        return _monstersSerializable;
    }

    void CheckMonsters(MonsterController _monsterDestroy, bool _isMenu)
    {
        if (_isMenu) return;

        _monsters.Remove(_monsterDestroy.gameObject);
        gameManager.CountKilledMonsters++;

        if (_monsters.Count == 0)
            gameManager.AllMonstersDied();
    }
    
    void DestroyMonsters()
    {
        foreach (GameObject _monsterObject in _monsters)
        {
            _monsterObject.GetComponent<MonsterController>().DestroyMonster();
        }

        _monsters = new();
    }
}
