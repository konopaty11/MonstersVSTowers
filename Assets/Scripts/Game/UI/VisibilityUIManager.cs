using UnityEngine;

public class VisibilityUIManager : MonoBehaviour
{
    public void ShowUI(RectTransform _rectTransform)
    {
        _rectTransform.gameObject.SetActive(true);
    }

    public void HideUI(RectTransform _rectTransfrom)
    {
        _rectTransfrom.gameObject.SetActive(false);
    }
}
