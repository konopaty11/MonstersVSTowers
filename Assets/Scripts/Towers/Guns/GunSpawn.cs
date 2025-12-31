using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// логика спавна орудий
/// </summary>
public class GunSpawn : MonoBehaviour
{
    [SerializeField] Transform gunParent;
    [SerializeField] List<GameObject> gunPrefabs;
    
    /// <summary>
    /// спавн орудия
    /// </summary>
    /// <param name="_type"></param>
    public void SpawnGun(GunType _type)
    {
        foreach (GameObject _gunPrefab in gunPrefabs)
        {
            GunController _gunPrefabController = _gunPrefab.GetComponent<GunController>();
            if (_gunPrefabController.Type == _type)
                Instantiate(_gunPrefab, gunParent);
        }
    }
}
