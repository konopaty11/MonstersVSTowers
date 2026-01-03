using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// логика спавна орудий
/// </summary>
public class GunSpawn : MonoBehaviour
{
    [SerializeField] Transform gunParent;
    [SerializeField] List<GameObject> gunPrefabs;
    [SerializeField] CollectMonsters collection;
    
    /// <summary>
    /// спавн орудия
    /// </summary>
    /// <param name="_type"></param>
    public GunController SpawnGun(GunType _type)
    {
        foreach (GameObject _gunPrefab in gunPrefabs)
        {
            GunController _gunPrefabController = _gunPrefab.GetComponent<GunController>();
            if (_gunPrefabController.Type == _type)
            {
                GameObject _gunObject = Instantiate(_gunPrefab, gunParent);
                GunController _gun = _gunObject.GetComponent<GunController>();
                _gun.Init(collection);

                return _gun;
            }
        }

        return null;
    }
}
