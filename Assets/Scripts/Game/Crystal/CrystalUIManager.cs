using TMPro;
using UnityEngine;

public class CrystalUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI crystalsText;
    [SerializeField] Crystals crystals;

    void OnEnable()
    {
        Crystals.OnCountCrystalsChange += UpdateUI;
    }

    void OnDisable()
    {
        Crystals.OnCountCrystalsChange -= UpdateUI;
    }

    void Start()
    {
        UpdateUI(crystals.crystals);
    }

    void UpdateUI(int _crystals)
    {
        crystalsText.text = _crystals.ToString();
    }
}
