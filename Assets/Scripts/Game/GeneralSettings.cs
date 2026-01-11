using UnityEngine;

[CreateAssetMenu(fileName = "GeneralSettings", menuName = "Scriptable Objects/GeneralSettings")]
public class GeneralSettings : ScriptableObject
{
    public float castleHealth;

    public float minSpawnDelay = 1.5f;
    public float maxSpawnDelay = 2f;
    public float delayBetweenWaves = 3f;
}
