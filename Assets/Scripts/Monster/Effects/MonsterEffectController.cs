using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void InitSlowEffect(List<Image> _slowEffectScale)
    {
        SlowEffect _slowEffect = (SlowEffect)GetEffect(EffectType.Slow);
        _slowEffect.SlowEffectScale = _slowEffectScale;
    }

    public void DestroyAllEffects()
    {
        foreach (MonsterEffect _effect in effects)
        {
            _effect.DestroyEffect();
        }
    }
}
