using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] Slider healthSlider;
    [SerializeField] List<Image> slowEffectScale;

    public Slider HealthSlider => healthSlider;

    public List<Image> SlowEffectScale => slowEffectScale;

    public Transform TargetMonster { get; private set; }

    Vector3 _startPosition;

    void Start()
    {
        _startPosition = transform.localPosition;
    }

    void Update()
    {
        transform.position = new
            (
            TargetMonster.position.x + _startPosition.x,
            _startPosition.y,
            TargetMonster.position.z + _startPosition.z
            );
    }

    public void Init(Transform _targetMonster)
    {
        TargetMonster = _targetMonster;
    }

}
