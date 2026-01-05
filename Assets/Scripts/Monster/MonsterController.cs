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
    [SerializeField] MonsterType type;
    [SerializeField] SplineAnimate spline;
    [SerializeField] MonstersSettings monstersSettings;
    [SerializeField] Slider healthSlider;
    [SerializeField] Animator animator;
    [SerializeField] Renderer monsterRenderer;
    [SerializeField] List<Transform> offsetObjects;

    public MonsterType Type => type;

    public Vector3 CurrentVelocity { get; private set; }
    Vector3 _previousPosition;

    float _currentHealth;
    MonsterSettings _settings;

    RotatingAndShoutingGuns _lastHittedGun;

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
        spline.MaxSpeed = _speedCoefficient * _settings.speed;
    }

    void CalculateVelocity()
    {
        CurrentVelocity = (transform.position - _previousPosition) / Time.deltaTime;
        _previousPosition = transform.position;
    }

    public void CartridgeHit(RotatingAndShoutingGuns _gun, float _damage)
    {
        _lastHittedGun = _gun;
        SubstractHealth(_damage);
    }

    public void InitMonster(SplineContainer _spline)
    {
        float _xOffset = Random.Range(_minXOffset, _maxXOffset);
        foreach (Transform _offsetObject in offsetObjects)
        {
            _offsetObject.position = new(_offsetObject.position.x + _xOffset, _offsetObject.position.y, _offsetObject.position.z);
        }

        spline.Container = _spline;
        spline.Play();

        foreach (MonsterSettings _monsterSettings in monstersSettings.monsters)
        {
            if (_monsterSettings.type == type)
            {
                _settings = _monsterSettings;
                break;
            }
        }

        spline.MaxSpeed = _settings.speed;
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
        spline.enabled = false;

        healthSlider.gameObject.SetActive(false);

        _lastHittedGun.Collection.HandleRemoveMonster(this);

        StartCoroutine(FadingMaterial(_duration));
        Destroy(gameObject, _duration);
    }

    IEnumerator FadingMaterial(float _duration)
    {
        Material _originMaterial = monsterRenderer.material;
        Material _materialCopy = new Material(_originMaterial);
        monsterRenderer.material = _materialCopy;

        Color _color = _materialCopy.color;
        float _startAlpha = _color.a;
        float _finishAlpha = 0f;

        float _elapsed = 0f;
        while (_elapsed < _duration)
        {
            _elapsed += Time.deltaTime;

            _color.a = Mathf.Lerp(_startAlpha, _finishAlpha, _elapsed / _duration);
            _materialCopy.color = _color;

            yield return null;
        }
    }
}
