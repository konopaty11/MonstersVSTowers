using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class MenuController : MonoBehaviour
{
    [Header("Monsters spawn")]
    [SerializeField] SplineContainer spline;
    [SerializeField] GameObject monsterPrefab;
    [SerializeField] Transform monsterParent;
    [SerializeField] GeneralSettings generalSettings;

    [Header("Guns spawn")]
    [SerializeField] List<TowerController> towers;

    List<GameObject> _monsterObjects = new();
    int _targetCountMonsters = 1;

    void OnEnable()
    {
        MonsterController.OnMonsterDestroy += CheckMonsters;
    }

    void OnDisable()
    {
        MonsterController.OnMonsterDestroy -= CheckMonsters;
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        foreach (TowerController _tower in towers)
        {
            _tower.HandleTowerInteraction(Modes.CreatingCannon);
        }

        StartCoroutine(SpawnControl());
    }

    IEnumerator SpawnControl()
    {
        while (true)
        {
            for (int i = _monsterObjects.Count; i < _targetCountMonsters; i++)
            {
                SpawnMonster();

                yield return new WaitForSeconds(Random.Range(generalSettings.minSpawnDelay, generalSettings.maxSpawnDelay));
            }

            yield return null;
        }
    }

    void SpawnMonster()
    {
        GameObject _monsterObject = Instantiate(monsterPrefab, monsterParent);
        _monsterObjects.Add(_monsterObject);

        MonsterController _monster = _monsterObject.GetComponent<MonsterController>();
        _monster.InitMonster(spline, true);
    }

    

    void CheckMonsters(MonsterController _monsterDestroy)
    {
        _monsterObjects.Remove(_monsterDestroy.gameObject);
    }
}
