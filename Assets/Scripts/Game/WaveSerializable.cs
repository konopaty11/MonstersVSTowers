using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class WaveSerializable : MonoBehaviour
{
    [SerializeField] List<MonsterSerializable> monsters;

    public List<MonsterSerializable> Monsters => monsters;
}

[Serializable]
public class MonsterSerializable
{
    public MonsterType type;
    public int count;
}
