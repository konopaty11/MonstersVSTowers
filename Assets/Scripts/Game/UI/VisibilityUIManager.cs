using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityUIManager : MonoBehaviour
{
    [SerializeField] List<VisibilityUISerializable> uiSerializers;

    public void ShowUI(string _id, ShowType _type)
    {
        foreach (VisibilityUISerializable _uiSerializer in uiSerializers)
        {
            if (_id == _uiSerializer.id)
            {
                switch (_type)
                {
                    case ShowType.Moving:
                        StartCoroutine(Moving(_uiSerializer.rectTransfrom, _uiSerializer.startPositionType, _uiSerializer.targetPositionType, _uiSerializer.duration));
                        break;
                    case ShowType.Fading:
                        StartCoroutine(Fading(_uiSerializer.canvasGroup, _uiSerializer.startAlpha, _uiSerializer.targetAlpha, _uiSerializer.duration));
                        break;
                }
            }
        }
    }

    public void HideUI(string _id, ShowType _type)
    {
        foreach (VisibilityUISerializable _uiSerializer in uiSerializers)
        {
            if (_id == _uiSerializer.id)
            {
                switch (_type)
                {
                    case ShowType.Moving:
                        StartCoroutine(Moving(_uiSerializer.rectTransfrom, _uiSerializer.targetPositionType, _uiSerializer.startPositionType, _uiSerializer.duration));
                        break;
                    case ShowType.Fading:
                        StartCoroutine(Fading(_uiSerializer.canvasGroup, _uiSerializer.targetAlpha, _uiSerializer.startAlpha, _uiSerializer.duration));
                        break;
                }
            }
        }
    }

    IEnumerator Moving(RectTransform _rectTransform, UIPositionType _startPositionType, UIPositionType _targetPositionType, float _duration)
    {
        Vector3 _startPosition = GetPositionFromPositionType(_rectTransform, _startPositionType);
        Vector3 _targetPosition = GetPositionFromPositionType(_rectTransform, _targetPositionType);

        float _elapsed = 0f;
        while (_elapsed <= _duration)
        {
            _elapsed += Time.deltaTime;

            _rectTransform.anchoredPosition = Vector3.Lerp(_startPosition, _targetPosition, _elapsed / _duration);

            yield return null;
        }

        _rectTransform.anchoredPosition = _targetPosition;
    }

    IEnumerator Fading(CanvasGroup _canvasGroup, float _startAlpha, float _targetAlpha, float _duration)
    {
        float _elapsed = 0f;
        while (_elapsed <= _duration)
        {
            _elapsed += Time.deltaTime;

            _canvasGroup.alpha = Mathf.Lerp(_startAlpha, _targetAlpha, _elapsed / _duration);

            yield return null;
        }

        _canvasGroup.alpha = _targetAlpha;
    }

    Vector3 GetPositionFromPositionType(RectTransform _rectTransform, UIPositionType _positionType)
    {
        return _positionType switch
        {
            UIPositionType.None => throw new ArgumentException("UIPosition None"),
            UIPositionType.Center => Vector3.zero,
            UIPositionType.Left => new(-_rectTransform.rect.width / 2 - Screen.width / 2, 0f),
            UIPositionType.Top => new(0f, _rectTransform.rect.height / 2 + Screen.height / 2),
            UIPositionType.Right => new(_rectTransform.rect.width / 2 + Screen.width / 2, 0f),
            UIPositionType.Bottom => new(0f, -_rectTransform.rect.height / 2 - Screen.height / 2)
        };
    }
}

[Serializable]
public class VisibilityUISerializable
{
    [Header("General settings")]
    public string id;
    public float duration;

    [Header("Moving settings")]
    public RectTransform rectTransfrom;
    public UIPositionType startPositionType;
    public UIPositionType targetPositionType;

    [Header("Fading settings")]
    public CanvasGroup canvasGroup;
    public float startAlpha;
    public float targetAlpha;
}