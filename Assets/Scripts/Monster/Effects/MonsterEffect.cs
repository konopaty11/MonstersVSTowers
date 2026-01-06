using UnityEngine;

public abstract class MonsterEffect : MonoBehaviour
{
    [SerializeField] protected EffectType type;
    [SerializeField] protected MonsterController monster;

    public EffectType Type => type;

    public abstract void StartEffect();
    public abstract void DestroyEffect();
}
