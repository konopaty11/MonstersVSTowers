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
            SpawnMonster();

            yield return new WaitForSeconds(Random.Range(generalSettings.minSpawnDelay, generalSettings.maxSpawnDelay));
        }
    }

    void SpawnMonster()
    {
        GameObject _monsterObject = Instantiate(monsterPrefab, monsterParent);
        MonsterController _monster = _monsterObject.GetComponent<MonsterController>();
        _monster.InitMonster(spline);
    }
}
