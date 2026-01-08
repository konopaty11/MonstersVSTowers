using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowersUpdate", menuName = "Scriptable Objects/TowersUpdateSerializable")]
public class TowersUpgradeSerializable : ScriptableObject
{
    public List<TowerLevelUpgradeSerializable> towers;
}

[Serializable]
public class TowerLevelUpgradeSerializable : LevelSettings
{
    public Mesh mesh;
    public float rangeMultiplier;
}