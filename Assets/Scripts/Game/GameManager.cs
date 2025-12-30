using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<WaveSerializable> waves;
    [SerializeField] MonstersSpawn monsterSpawn;

    int _currentWave;

    float _minSpawnDelay = 1f;
    float _maxSpawnDelay = 1.5f;

    IEnumerator Game()
    {
        foreach (MonsterSerializable _monster in waves[_currentWave].Monsters)
        {
            for (int i = 0; i < _monster.count; i++)
            {
                monsterSpawn.SpawnMonster(_monster.type);

                yield return new WaitForSeconds(Random.Range(_minSpawnDelay, _maxSpawnDelay));
            }
        }
    }
}
