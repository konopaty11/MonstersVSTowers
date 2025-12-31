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

    /// <summary>
    /// спавн монстров
    /// </summary>
    /// <param name="_type"> тип монстра </param>
    public void SpawnMonster(MonsterType _type)
    {
        foreach (GameObject _monsterPrefab in monsterPrefabs)
        {
            MonsterController _controller = _monsterPrefab.GetComponent<MonsterController>();
            if (_controller.Type == _type)
            {
                GameObject _monster = Instantiate(_monsterPrefab, monsterParent);

                MonsterController _monsterController = _monster.GetComponent<MonsterController>();
                _monsterController.InitMonster(spline);
            }
        }    
    }
}
