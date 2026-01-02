using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// управление режимом контроля орудий и башен
/// </summary>
public class ModeManager : MonoBehaviour
{
    [SerializeField] List<ChangeModeButtonSerializable> buttons;

    public static UnityAction<Modes> OnModeChange;

    public Modes Mode { get; private set; }

    void Start()
    {
        SetListeners();
    }

    void SetListeners()
    {
        foreach (ChangeModeButtonSerializable _modeButton in buttons)
        {
            _modeButton.button.onClick.AddListener(() => SetMode(_modeButton.mode));
        }
    }

    public void SetMode(Modes _mode)
    {
        Mode = _mode;
        OnModeChange?.Invoke(Mode);
    }

    public static bool IsCreatingMode(Modes _mode)
    {
        return _mode >= Modes.CreatingCannon;
    }
}

[Serializable]
class ChangeModeButtonSerializable
{
    public Button button;
    public Modes mode;
}
