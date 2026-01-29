using UnityEngine;
using UnityEngine.EventSystems;

public class CreateButtonModeController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] Modes mode;
    [SerializeField] ModeManager modeManager;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] GameManager gameManager;

    Vector2 _starPosition;

    void Start()
    {
        _starPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (modeManager.Mode != mode)
            modeManager.SetModeControl(mode);

        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (
                rectTransform.parent as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 _localPoint
            );

        rectTransform.localPosition = _localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        gameManager.ThrowRaycast(eventData.position);
        modeManager.SetModeControl(Modes.None);
        rectTransform.anchoredPosition = _starPosition;
    }
}
