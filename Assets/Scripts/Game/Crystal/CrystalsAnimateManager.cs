using System.Collections;
using TMPro;
using UnityEngine;

public class CrystalsAnimateManager : MonoBehaviour
{
    [SerializeField] GameObject deltaCrystalsPrefab;
    [SerializeField] Color positiveTargetColor;
    [SerializeField] Color negativeTargetColor;

    float _heightFlying = 2.5f;
    float _durationAnimation = 4f;

    void Awake()
    {
        ServiceLocator.Register(this);
    }

    public void DeltaCrystalsPositionAnimate(Vector3 _startPosition, int _crystals)
    {
        GameObject _deltaCrystalsObject = Instantiate(deltaCrystalsPrefab, _startPosition, deltaCrystalsPrefab.transform.rotation);
        TextMeshPro _text = _deltaCrystalsObject.GetComponent<TextMeshPro>();

        _text.text = _crystals <= 0 ? _crystals.ToString() : "+" + _crystals.ToString();
        Color _targetColor = _crystals <= 0 ? negativeTargetColor : positiveTargetColor;
        Vector3 _targetPosition = _startPosition + Vector3.left * _heightFlying;

        StartCoroutine(DeltaPositionAnimation
            (
            _deltaCrystalsObject.transform,
            _startPosition,
            _targetPosition,
            _text,
            _text.color,
            _targetColor,
            _durationAnimation
            ));
    }

    IEnumerator DeltaPositionAnimation(Transform _deltaCrystalsTransform, Vector3 _startPosition, Vector3 _targetPosition, TextMeshPro _text, Color _startColor, Color _targetColor, float _duration)
    {
        float _elapsed = 0f;
        while (_elapsed < _duration)
        {
            _elapsed += Time.deltaTime;

            _deltaCrystalsTransform.position = Vector3.Slerp(_startPosition, _targetPosition, _elapsed / _duration);
            _text.color = Color.Lerp(_startColor, _targetColor, _elapsed / _duration);

            yield return null;
        }

        Destroy(_deltaCrystalsTransform.gameObject);
    }

}
