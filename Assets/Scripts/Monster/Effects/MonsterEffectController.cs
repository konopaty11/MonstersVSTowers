using System.Collections.Generic;
using UnityEngine;

public class MonsterEffectController : MonoBehaviour
{
    [SerializeField] List<MonsterEffect> effects;

    public MonsterEffect GetEffect(EffectType _type)
    {
        foreach (MonsterEffect _effect in effects)
        {
            if (_effect.Type == _type)
                return _effect;
        }

        throw new MissingReferenceException($"Missing {_type} monster effect in the Monster effects manager.");
    }

    public void DestroyAllEffects()
    {
        foreach (MonsterEffect _effect in effects)
        {
            _effect.DestroyEffect();
        }
    }
}
