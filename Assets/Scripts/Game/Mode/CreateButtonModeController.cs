using UnityEngine;
using UnityEngine.EventSystems;

public class CreateButtonModeController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] Modes mode;
    [SerializeField] ModeManager modeManager;
    [SerializeField] RectTransform rectTransform;

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition = eventData.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        modeManager.SetModeControl(mode);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        modeManager.SetModeControl(Modes.None);
    }
}
