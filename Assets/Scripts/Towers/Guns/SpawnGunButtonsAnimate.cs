using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// логика анимаций кнопок
/// </summary>
public class SpawnGunButtonsAnimate : MonoBehaviour
{
    [Header("Gun type")]
    [SerializeField] GunType type;

    [Header("Towers")]
    [SerializeField] Material towersMaterial;
    [SerializeField] Color pressedColor;

    [Header("Create Button")]
    [SerializeField] Image createBtn;
    [SerializeField] Sprite createBtnNotPressed;
    [SerializeField] Sprite createBtnPressed;

    [Header("Gun Renderer")]
    [SerializeField] Transform gunRenderer;
    [SerializeField] float speedRotate = 20f;

    bool _isPress = false;
    Coroutine _rotateGunRendereCoroutine;
    Vector3 _baseGunRendererRotation;

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
        Init();
    }

    void Init()
    {
        _baseGunRendererRotation = gunRenderer.eulerAngles;
    }

    void UpdateMode(Modes _mode)
    {
        if ((GunType)_mode != type)
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

    /// <summary>
    /// зажатие кнопки
    /// </summary>
    void SetPressedBtn()
    {
        createBtn.sprite = createBtnPressed;
        _rotateGunRendereCoroutine = StartCoroutine(RotateGunRenderer());
    }

    /// <summary>
    /// отжатие кнопки
    /// </summary>
    void SetNotPressedBtn()
    {
        createBtn.sprite = createBtnNotPressed;
        gunRenderer.transform.eulerAngles = _baseGunRendererRotation;
        if (_rotateGunRendereCoroutine != null)
            StopCoroutine(_rotateGunRendereCoroutine);
    }

    /// <summary>
    /// вращение орудия
    /// </summary>
    /// <returns></returns>
    IEnumerator RotateGunRenderer()
    {
        while (true)
        {
            gunRenderer.Rotate(Vector3.up, speedRotate * Time.deltaTime);
            yield return null;
        }
    }
}
