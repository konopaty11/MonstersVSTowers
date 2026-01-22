using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] VisibilityUIManager visibilityUIManager;

    string _pauseID = "Pause";

    public void OpenPause()
    {
        visibilityUIManager.ShowUI(_pauseID, ShowType.Moving);
    }

    public void ClosePause()
    {
        visibilityUIManager.HideUI(_pauseID, ShowType.Moving);
    }
}
