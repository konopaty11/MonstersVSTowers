using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class ConsoleManager : MonoBehaviour
{
    [SerializeField] InputField inputField;

    Vector2 _startPosition;
    Vector2 _finishPosition;

    float _distanceThreshold = 100f;
    float _diagonalThreshold = 0.4f;

    Dictionary<string, ConsoleCommand> _comands = new();

    void Update()
    {
        ReadTouch();

    }

    void ReadTouch()
    {
        if (Touchscreen.current == null) return;

        TouchControl _touch = Touchscreen.current.primaryTouch;

        if (_touch.press.wasPressedThisFrame)
        {
            _startPosition = _touch.ReadValue().position;
        }

        if (_touch.press.wasReleasedThisFrame)
        {
            _finishPosition = _touch.ReadValue().position;
            DetectDiagonalSwipe();
        }
    }

    void DetectDiagonalSwipe()
    {
        Vector2 _swipeVector = _finishPosition - _startPosition;
        if (_swipeVector.magnitude < _distanceThreshold) return;

        Vector2 _swipeVectorNormalized = _swipeVector.normalized;
        if (_swipeVectorNormalized.x < _diagonalThreshold || 
            _swipeVectorNormalized.y < _diagonalThreshold) return;
    }

    public void ExecuteCommand()
    {

    }
}
