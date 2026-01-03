using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RotatingAndShoutingGunsSettings", menuName = "Scriptable Objects/RotatingAndShoutingGunsSettings")]
public class RotatingAndShoutingGunsSettings : ScriptableObject
{
    public List<RotatingAndShoutingGunSettingsSerializable> guns;
}

[Serializable]
public class RotatingAndShoutingGunSettingsSerializable
{
    public GunType type;
    public List<RotatingAndShoutingGunLevelSettingsSerializable> levels;
}

[Serializable]
public class RotatingAndShoutingGunLevelSettingsSerializable : GunLevelSettingsSerializable
{
    public int countGuns;
}