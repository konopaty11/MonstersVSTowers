using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int crystals;
    public int wave;
    public float volume;
    public float castleHealth;
    public List<TowerSerializable> towers = new();
    public List<MonsterSerializable> monsters = new();
    public bool isWaveSave;

    public int countKilledMonsters;
    public int countCreatedGuns;
    public int ñountUpdatedGuns;
    public int ñountUpdatedTowers;
    public float timer;

    public SaveData(int _crystals, float _volume, float _castleHealth, int _wave, List<TowerSerializable> _towers)
    {
        crystals = _crystals;
        volume = _volume;
        wave = _wave;
        castleHealth = _castleHealth;
        towers = _towers;
    }

    public SaveData() { }
}

[Serializable]
public class TowerSerializable
{
    public int id;
    public int level;
    public GunType gunType = GunType.None;
    public int gunLevel;

    public TowerSerializable(int _id)
    {
        id = _id;
    }

    public TowerSerializable() { }
}

[Serializable]
public class MonsterSerializable
{
    public MonsterType monsterType;
    public float health;
    public float normalizePosition;
    public bool isDied;

    public MonsterSerializable(MonsterType type, float _health)
    {
        monsterType = type;
        health = _health;
    }

    public MonsterSerializable() { }
}
