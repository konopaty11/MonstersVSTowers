using System;
using TMPro;
using UnityEngine;

public class ResultWindowController : MonoBehaviour
{
    [SerializeField] GameObject canvasObject;
    [SerializeField] TextMeshProUGUI currentWaveText;
    [SerializeField] TextMeshProUGUI countKilledMonstersText;
    [SerializeField] TextMeshProUGUI countCreatedGunsText;
    [SerializeField] TextMeshProUGUI countUpdatedGunsText;
    [SerializeField] TextMeshProUGUI countUpdatedTowersText;
    [SerializeField] TextMeshProUGUI timerText;

    public void SetActive(bool _active)
    {
        canvasObject.SetActive(_active);
    }

    public void UpdateUI(int _currentWave, int _countKilledMonsters, int _countCreatedGuns, int _countUpdatedGuns, int _countUpdatedTowers, float _timer)
    {
        currentWaveText.text = _currentWave.ToString();
        countKilledMonstersText.text = _countKilledMonsters.ToString();
        countCreatedGunsText.text = _countCreatedGuns.ToString();
        countUpdatedGunsText.text = _countUpdatedGuns.ToString();
        countUpdatedTowersText.text = _countUpdatedTowers.ToString();

        TimeSpan _span = TimeSpan.FromSeconds(_timer);
        timerText.text = _span.ToString(@"m\:ss");
    }
}
