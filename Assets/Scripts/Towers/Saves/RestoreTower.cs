using UnityEngine;

public class RestoreTower : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] TowerController tower;

    public int ID => id;

    TowerSerializable _currentTowerSerializable;
    public TowerSerializable TowerSerializable => _currentTowerSerializable;

    void Awake()
    {
        _currentTowerSerializable = new(ID);
    }

    public void LoadData(SaveData _saveData)
    {
        foreach (TowerSerializable _towerSerializable in _saveData.towers)
        {
            if (_towerSerializable.id == id)
                _currentTowerSerializable = _towerSerializable;
        }

        for (int i = 1; i < _currentTowerSerializable.level; i++)
            tower.Upgrade();

        if (_currentTowerSerializable.gunType == GunType.None) return;
        tower.CreateGun(_currentTowerSerializable.gunType);

        for (int i = 1; i < _currentTowerSerializable.gunLevel; i++)
            tower.CurrentGun.Upgrade();
    }
}
