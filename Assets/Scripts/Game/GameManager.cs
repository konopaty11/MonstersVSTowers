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

    float _minSpawnDelay = 5f;
    float _maxSpawnDelay = 5.5f;

    string _towerTag = "Tower";

    InputSystem_Actions _inputSystem;

    void Awake()
    {
        _inputSystem = new();
    }

    void OnEnable()
    {
        _inputSystem.Player.Attack.Enable();
        _inputSystem.Player.Attack.performed += ThrowRaycast; 
    }

    void OnDisable()
    {
        _inputSystem.Player.Attack.Disable();
        _inputSystem.Player.Attack.performed -= ThrowRaycast;
    }

    void Start()
    {
        StartCoroutine(Game());
    }

    void ThrowRaycast(InputAction.CallbackContext _context)
    {
        if (Touchscreen.current == null || modeManager.Mode == Modes.None) return;

        foreach (TouchControl _touch in Touchscreen.current.touches)
        {
            Vector2 _position = _touch.position.ReadValue();
            Ray _ray = mainCamera.ScreenPointToRay(_position);

            if (Physics.Raycast(_ray, out RaycastHit _hit) && _hit.collider.CompareTag(_towerTag))
            {
                TowerController _tower = _hit.collider.GetComponent<TowerController>();
                _tower.HandleTowerInteraction(modeManager.Mode);
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
