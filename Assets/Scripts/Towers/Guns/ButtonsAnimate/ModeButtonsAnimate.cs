using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ModeButtonsAnimate : MonoBehaviour
{
    [Header("Type")]
    [SerializeField] protected Modes type;

    [Header("Create Button")]
    [SerializeField] protected Image createBtn;
    [SerializeField] protected Sprite createBtnNotPressed;
    [SerializeField] protected Sprite createBtnPressed;

    bool _isPress = false;

    List<Transform> _childTransforms = new();

    void OnEnable()
    {
        ModeManager.OnModeChange += UpdateMode;
    }

    void OnDisable()
    {
        ModeManager.OnModeChange -= UpdateMode;
    }

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform _childTransform = transform.GetChild(i);
            _childTransforms.Add(_childTransform);
        }
    }

    void UpdateMode(Modes _mode)
    {
        if (_mode != type)
            SetNotPressedBtn();
    }

    /// <summary>
    /// обработка нажатия на кнопку
    /// </summary>
    public void OnPress()
    {
        _isPress = !_isPress;

        if (_isPress)
        {
            SetPressedBtn();
        }
        else
        {
            SetNotPressedBtn();
        }
    }

    protected virtual void SetPressedBtn()
    {
        _isPress = true;
        createBtn.sprite = createBtnPressed;
    }
    protected virtual void SetNotPressedBtn()
    {
        _isPress = false;
        createBtn.sprite = createBtnNotPressed;
    }
}
