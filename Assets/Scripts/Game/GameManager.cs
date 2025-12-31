using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

/// <summary>
/// Основной менеджер
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] ModeManager modeManager;
    [SerializeField] WavesSerializable waves;
    [SerializeField] MonstersSpawn monsterSpawn;

    int _currentWave;

    float _minSpawnDelay = 1f;
    float _maxSpawnDelay = 1.5f;

    void Start()
    {
        StartCoroutine(Game());
    }

    void Update()
    {
        if (modeManager.Mode != Modes.None)
            ThrowRaycast();
    }

    void ThrowRaycast()
    {
        if (Touchscreen.current == null) return;

        foreach (TouchControl _touch in Touchscreen.current.touches)
        {
            Vector2 _position = _touch.position.ReadValue();
            Ray _ray = mainCamera.ScreenPointToRay(_position);

            if (Physics.Raycast(_ray, out RaycastHit _hit))
            {

            }
        }
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
