using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Основной менеджер
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] WavesSerializable waves;
    [SerializeField] MonstersSpawn monsterSpawn;

    int _currentWave;

    float _minSpawnDelay = 1f;
    float _maxSpawnDelay = 1.5f;

    void Start()
    {
        StartCoroutine(Game());
    }

    /// <summary>
    /// корутина основного гемплея
    /// </summary>
    /// <returns></returns>
    IEnumerator Game()
    {
        foreach (MonsterWaveSerializable _monster in waves.waves[_currentWave].monsters)
        {
            for (int i = 0; i < _monster.count; i++)
            {
                monsterSpawn.SpawnMonster(_monster.type);

                yield return new WaitForSeconds(Random.Range(_minSpawnDelay, _maxSpawnDelay));
            }
        }
    }
}
