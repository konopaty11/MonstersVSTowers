using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunsSettings", menuName = "Scriptable Objects/GunsSettings")]
public class GunsSettings : ScriptableObject
{
    public List<GunSettingsSerializable> guns;
}

[Serializable]
public class GunSettingsSerializable
{
    public GunType type;
    public List<GunLevelSettingsSerializable> levels;
}

[Serializable]
public class GunLevelSettingsSerializable
{
    public int level;
    public float radius;
    public float damage;
    public float attackInterval;
}