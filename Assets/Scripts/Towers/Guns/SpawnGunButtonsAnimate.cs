using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// логика анимаций кнопок
/// </summary>
public class SpawnGunButtonsAnimate : MonoBehaviour
{
    [Header("Towers")]
    [SerializeField] Material towersMaterial;
    [SerializeField] Color pressedColor;

    [Header("Create Button")]
    [SerializeField] Image createBtn;
    [SerializeField] Sprite createBtnNotPressed;
    [SerializeField] Sprite createBtnPressed;

    [Header("Gun Renderer")]
    [SerializeField] Transform gunRenderer;
    [SerializeField] float speedRotate = 5f;

    bool _isPress = false;
    Coroutine _rotateGunRendereCoroutine;
    Vector3 _baseGunRendererRotation;

    void Start()
    {
        Init();
    }

    void Init()
    {
        _baseGunRendererRotation = gunRenderer.eulerAngles;
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
        towersMaterial.color = pressedColor;
        createBtn.sprite = createBtnPressed;
        _rotateGunRendereCoroutine = StartCoroutine(RotateGunRenderer());
    }

    /// <summary>
    /// отжатие кнопки
    /// </summary>
    void SetNotPressedBtn()
    {
        towersMaterial.color = Color.white;
        createBtn.sprite = createBtnNotPressed;
        StopCoroutine(_rotateGunRendereCoroutine);
        gunRenderer.transform.eulerAngles = _baseGunRendererRotation;
    }

    /// <summary>
    /// вращение орудия
    /// </summary>
    /// <returns></returns>
    IEnumerator RotateGunRenderer()
    {
        while (true)
        {
            gunRenderer.Rotate(Vector3.back, speedRotate * Time.deltaTime);
            yield return null;
        }
    }

    
}
