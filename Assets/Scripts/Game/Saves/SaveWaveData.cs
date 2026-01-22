using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveWaveData
{
    public int wave;
    public float castleHealth;
    public List<TowerSerializable> towers = new();
    public List<MonsterSerializable> monsters = new();
}