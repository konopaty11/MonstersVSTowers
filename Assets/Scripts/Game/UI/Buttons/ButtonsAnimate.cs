using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonsAnimate : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Create Button")]
    [SerializeField] protected Image createBtn;
    [SerializeField] protected Sprite createBtnNotPressed;
    [SerializeField] protected Sprite createBtnPressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        createBtn.sprite = createBtnPressed;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        createBtn.sprite = createBtnNotPressed;
    }
}
