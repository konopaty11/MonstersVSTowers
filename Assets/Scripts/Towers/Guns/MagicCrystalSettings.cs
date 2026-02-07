using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicCrystalSettings", menuName = "Scriptable Objects/MagicCrystalSettings")]
public class MagicCrystalSettings : ScriptableObject
{
    public List<MagicCrystalLevelSettingsSerializable> levels;
}

[Serializable]
public class MagicCrystalLevelSettingsSerializable : GunLevelSettingsSerializable
{
    public bool isExistStone;
    public Mesh stonesMesh;
    public float slowSpeedCoefficient;
}