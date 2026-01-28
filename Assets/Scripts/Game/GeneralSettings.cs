using UnityEngine;

[CreateAssetMenu(fileName = "GeneralSettings", menuName = "Scriptable Objects/GeneralSettings")]
public class GeneralSettings : ScriptableObject
{
    public float castleHealth;

    public float minSpawnDelay = 1.5f;
    public float maxSpawnDelay = 2f;
    public float delayBetweenWaves = 3f;
    public float timeThresholdForStar = 100f;
    public int countFullStarsForRefard = 3;
    public int countCrystalsAsRefard = 100;
}
