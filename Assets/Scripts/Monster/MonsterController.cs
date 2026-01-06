using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

/// <summary>
/// логика монстра
/// </summary>
public class MonsterController : MonoBehaviour
{
    [Header("Type")]
    [SerializeField] MonsterType type;

    [Header("Movement")]
    [SerializeField] SplineFollow splineFollow;
    [SerializeField] List<Transform> offsetObjects;

    [Header("Settings")]
    [SerializeField] MonstersSettings monstersSettings;

    [Header("Animation")]
    [SerializeField] Slider healthSlider;
    [SerializeField] Animator animator;

    [Header("Rendering")]
    [SerializeField] Renderer monsterRenderer;
    [SerializeField] Material diedMaterial;

    [Header("Effect")]
    [SerializeField] MonsterEffectController monsterEffectController;

    public MonsterType Type => type;

    public GunController LastAttackedGun { get; set; }

    public Vector3 CurrentVelocity { get; private set; }
    Vector3 _previousPosition;

    float _currentHealth;
    MonsterSettings _settings;

    float _minXOffset = -0.5f;
    float _maxXOffset = 0.5f;

    float _speedCoefficient = 1f;


    void Update()
    {
        CalculateVelocity();
    }

    public void SetSpeedCoefficient(float _newCoefficient)
    {
        _speedCoefficient = _newCoefficient;
        splineFollow.Speed = _speedCoefficient * _settings.speed;
    }

    void CalculateVelocity()
    {
        CurrentVelocity = (transform.position - _previousPosition) / Time.deltaTime;
        _previousPosition = transform.position;
    }

    public void InitMonster(SplineContainer _spline)
    {
        float _xOffset = Random.Range(_minXOffset, _maxXOffset);
        foreach (Transform _offsetObject in offsetObjects)
        {
            _offsetObject.position = new(_offsetObject.position.x + _xOffset, _offsetObject.position.y, _offsetObject.position.z);
        }

        splineFollow.Container = _spline;

        foreach (MonsterSettings _monsterSettings in monstersSettings.monsters)
        {
            if (_monsterSettings.type == type)
            {
                _settings = _monsterSettings;
                break;
            }
        }

        splineFollow.Speed = _settings.speed;
        _currentHealth = _settings.health;
    }

    public void SubstractHealth(float _damage)
    {
        _currentHealth -= _damage;
        if (_currentHealth <= 0)
        {
            DestroyMonster();
            return;
        }

        healthSlider.value = _currentHealth / _settings.health;
    }

    void DestroyMonster()
    {
        float _duration = 5f;
        CapsuleCollider _collider = GetComponent<CapsuleCollider>();

        animator.enabled = false;
        _collider.enabled = false;
        splineFollow.enabled = false;
        
        healthSlider.gameObject.SetActive(false);

        LastAttackedGun.Collection.HandleRemoveMonster(this);
        monsterEffectController.DestroyAllEffects();

        StartCoroutine(FadingMaterial(_duration));
        Destroy(gameObject, _duration);
    }

    IEnumerator FadingMaterial(float _duration)
    {
        Material _copyMaterial = SetCopyMaterial();

        Color _color = _copyMaterial.color;
        float _startAlpha = _color.a;
        float _finishAlpha = 0f;

        float _elapsed = 0f;
        while (_elapsed < _duration)
        {
            _elapsed += Time.deltaTime;

            _color.a = Mathf.Lerp(_startAlpha, _finishAlpha, _elapsed / _duration);
            _copyMaterial.color = _color;

            yield return null;
        }
    }

    Material SetCopyMaterial()
    {
        Material _copyMaterial = new(diedMaterial);
        monsterRenderer.material = _copyMaterial;
        return _copyMaterial;
    }
}
