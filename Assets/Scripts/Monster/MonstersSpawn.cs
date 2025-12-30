using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstersSpawn : MonoBehaviour
{
    [SerializeField] Transform monsterParent;
    [SerializeField] List<GameObject> monsterPrefabs;

    List<MonsterController> _prefabControllers;

    public void SpawnMonster(MonsterType _type)
    {
        foreach (GameObject _monser in monsterParent)
        {
            MonsterController _controller = _monser.GetComponent<MonsterController>();
            if (_controller.Type == _type)
            {
                Instantiate(_monser, monsterParent);
            }
        }    
    }
}
