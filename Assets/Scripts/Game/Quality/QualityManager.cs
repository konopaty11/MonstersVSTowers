using TMPro;
using UnityEngine;

public class QualityManager : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;

    void Start()
    {
        Init();
    }

    void Init()
    {
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        foreach (string _qualityName in QualitySettings.names)
        {
            TMP_Dropdown.OptionData _option = new(_qualityName);
            dropdown.options.Add(_option);
        }

        dropdown.value = QualitySettings.GetQualityLevel();
    }

    void OnDropdownValueChanged(int _value)
    {
        QualitySettings.SetQualityLevel(_value);
    }
}
