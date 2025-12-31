using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowersUpdate", menuName = "Scriptable Objects/TowersUpdateSerializable")]
public class TowersUpgradeSerializable : ScriptableObject
{
    public List<TowerUpgradeSerializable> towers;
}

[Serializable]
public class TowerUpgradeSerializable
{
    public Mesh mesh;
    public float rangeMultiplier;
}