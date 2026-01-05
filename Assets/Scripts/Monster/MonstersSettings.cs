using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonstersSettings", menuName = "Scriptable Objects/MonstersSettings")]
public class MonstersSettings : ScriptableObject
{
    public List<MonsterSettings> monsters;
}

[Serializable]
public class MonsterSettings
{
    public MonsterType type;
    public float health;
    public float speed;
}