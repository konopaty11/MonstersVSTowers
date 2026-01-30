using TMPro;
using UnityEngine;

public class WaveResultWindowController : MonoBehaviour
{
    [SerializeField] VisibilityUIManager visibilityUIManager;
    [SerializeField] StarsController starsController;
    [SerializeField] GameObject waveWindowObject;
    [SerializeField] TextMeshProUGUI crystalsText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI energyText;

    string _waveID = "Wave";

    public void OpenWindow(float _time, int _crystals, int _energy, int _countStars)
    {
        waveWindowObject.SetActive(true);

        timeText.text = _time.ToString();
        crystalsText.text = _crystals.ToString();
        energyText.text = _energy.ToString();

        visibilityUIManager.ShowUI(_waveID, ShowType.Moving);
        starsController.ShowStars(_countStars);
    }
}
