using System.Collections;
using TMPro;
using UnityEngine;

public class WaveUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI wavesText;
    [SerializeField] TextMeshProUGUI wavesAnimatedText;

    RectTransform _waveAnimatedRectTransfrom;
    Vector2 _startPosition;
    Vector2 _targetPosition;

    string _patternWaveText = "Âîëíà: ";
    string _patternWaveAnimatedText = "ÂÎËÍÀ ";

    void OnEnable()
    {
        GameManager.OnUpdateWave += OnUpdateWave;
    }

    void OnDisable()
    {
        GameManager.OnUpdateWave -= OnUpdateWave;
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        _waveAnimatedRectTransfrom = wavesAnimatedText.GetComponent<RectTransform>();

        float _widthWaveAnimated = _waveAnimatedRectTransfrom.rect.width;
        _startPosition = new(Screen.width + _widthWaveAnimated / 2f, 0f);
        _targetPosition = new(-_widthWaveAnimated / 2f, 0f);
    }

    void OnUpdateWave(int _currentWave)
    {
        StartCoroutine(WavesTextAnimate(_currentWave));
    }

    IEnumerator WavesTextAnimate(int _currentWave)
    {
        wavesAnimatedText.text = _patternWaveAnimatedText + _currentWave;
        _waveAnimatedRectTransfrom.anchoredPosition = _startPosition;

        float _duration = 2f;
        float _elapsed = 0f;
        float _speed;
        while (_elapsed < _duration)
        {
            if (_elapsed / _duration >= 0.45f && _elapsed / _duration <= 0.55f)
                _speed = 0.2f;
            else
                _speed = 1f;

            _elapsed += Time.deltaTime * _speed;

            _waveAnimatedRectTransfrom.anchoredPosition = Vector2.Lerp(_startPosition, _targetPosition, _elapsed / _duration);

            yield return null;
        }

        UpdateWaveText(_currentWave);
    }

    void UpdateWaveText(int _currentWave)
    {
        wavesText.text = _patternWaveText + _currentWave;
    }
}
