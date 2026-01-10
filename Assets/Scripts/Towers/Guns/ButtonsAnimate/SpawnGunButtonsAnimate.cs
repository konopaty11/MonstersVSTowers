using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// логика анимаций кнопок
/// </summary>
public class SpawnGunButtonsAnimate : ModeButtonsAnimate
{
    [Header("Gun Renderer")]
    [SerializeField] protected Transform gunRenderer;
    [SerializeField] protected float speedRotate = 20f;

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
    /// зажатие кнопки
    /// </summary>
    protected override void SetPressedBtn()
    {
        base.SetPressedBtn();
        _rotateGunRendereCoroutine = StartCoroutine(RotateGunRenderer());
    }

    /// <summary>
    /// отжатие кнопки
    /// </summary>
    protected override void SetNotPressedBtn()
    {
        base.SetNotPressedBtn();
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
