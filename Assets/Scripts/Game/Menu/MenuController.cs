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
    [SerializeField] GameObject healthBarPrefab;
    [SerializeField] Transform healthBarParent;

    [Header("Guns spawn")]
    [SerializeField] List<TowerController> towers;

    [Header("Menu UI")]
    [SerializeField] CanvasGroup loadGameCanvasGroup;

    List<GameObject> _monsterObjects = new();
    int _targetCountMonsters = 5;

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
            _tower.CreateGun(GunType.Cannon);
        }

        StartCoroutine(SpawnControl());
    }

    public void LoadGameButtonActive(bool _active)
    {
        loadGameCanvasGroup.interactable = _active;
        loadGameCanvasGroup.alpha = _active ? 1 : 0;
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

        GameObject _healthBarObject = Instantiate(healthBarPrefab, healthBarParent);
        HealthBarController _healthBar = _healthBarObject.GetComponent<HealthBarController>();
        _healthBar.Init(_monsterObject.transform);
        MonsterController _monster = _monsterObject.GetComponent<MonsterController>();
        _monster.InitMonster(spline, _healthBar, null, true, true);
    }
 
    void CheckMonsters(MonsterController _monsterDestroy, bool _isMenu)
    {
        if (!_isMenu) return;

        _monsterObjects.Remove(_monsterDestroy.gameObject);
    }
}
