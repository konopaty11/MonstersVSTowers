using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Waves", menuName = "Scriptable Objects/WavesSerializable")]
public class WavesSerializable : ScriptableObject
{
    public List<Wave> waves;
}

[Serializable]
public class Wave
{
    public List<MonsterWaveSerializable> monsters;
}

[Serializable]
public class MonsterWaveSerializable
{
    public MonsterType type;
    public int count;
}