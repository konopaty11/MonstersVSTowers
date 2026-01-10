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

    void OnEnable()
    {
        ModeManager.OnModeChange += UpdateMode;
    }

    void OnDisable()
    {
        ModeManager.OnModeChange -= UpdateMode;
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
