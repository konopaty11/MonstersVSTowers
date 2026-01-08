using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlowEffect : MonsterEffect
{
    [SerializeField] float durationEffect = 4f;
    [SerializeField] Renderer monsterRenderer;
    [SerializeField] Color slowEffectColor;
    [SerializeField] List<Image> slowEffectScale;

    public float SlowSpeedCoefficient { get; set; }
    float _normalSpeedCoefficient = 1f;

    Coroutine _slowEffectControlCoroutine;

    Material _originMaterial;
    Material _slowEffectMaterial;

    Color _originColor;

    void Start()
    {
        _originMaterial = monsterRenderer.material;
        _slowEffectMaterial = new(_originMaterial);

        _originColor = _originMaterial.color;
    }

    public override void StartEffect()
    {
        if (_slowEffectControlCoroutine != null)
            StopCoroutine(_slowEffectControlCoroutine);

        StartCoroutine(SlowEffectControl());
    }

    public override void DestroyEffect()
    {
        foreach (Image _image in slowEffectScale)
        {
            _image.gameObject.SetActive(false);
        }
        StopAllCoroutines();
    }

    IEnumerator SlowEffectControl()
    {
        monsterRenderer.material = _slowEffectMaterial;
        monster.SetSpeedCoefficient(SlowSpeedCoefficient);
        SlowEffectColorControl(slowEffectColor);
        SetFullFillAmount();

        yield return WaitForSlowEffectScale();

        monster.SetSpeedCoefficient(_normalSpeedCoefficient);
        SlowEffectColorControl(_originColor);
        monsterRenderer.material = _originMaterial;

        _slowEffectControlCoroutine = null;
    }

    void SetFullFillAmount()
    {
        float _fullFillAmount = 1f;

        foreach (Image _image in slowEffectScale)
        {
            _image.fillAmount = _fullFillAmount;
        }
    }

    IEnumerator WaitForSlowEffectScale()
    {
        float _speed = 1f / durationEffect;
        float _targetFillAmount = 0f;

        while (slowEffectScale[0].fillAmount > 0)
        {
            foreach (Image _image in slowEffectScale)
            {
                _image.fillAmount = Mathf.MoveTowards(_image.fillAmount, _targetFillAmount, _speed * Time.deltaTime);
            }

            yield return null;
        }
    }

    IEnumerator SlowEffectColorControl(Color _targetColor)
    {
        if (_slowEffectMaterial.color == _targetColor)
            yield break;

        Color _startColor = _slowEffectMaterial.color;
        float _duration = 0.25f;
        float _elapsed = 0f;

        while (_elapsed < _duration)
        {
            _elapsed += Time.deltaTime;

            _slowEffectMaterial.color = Color.Lerp(_startColor, _targetColor, _elapsed / _duration);

            yield return null;
        }
    }
}
