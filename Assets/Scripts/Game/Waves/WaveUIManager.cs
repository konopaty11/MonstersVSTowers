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
    Vector2 _offsetPosition = new(10f, 0f);

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

        float _screenWidth = GameManager.GetRealScreenSize().x;

        float _widthWaveAnimated = _waveAnimatedRectTransfrom.rect.width;
        _startPosition = new(_screenWidth + _widthWaveAnimated / 2f, 0f);
        _targetPosition = new(-_widthWaveAnimated / 2f, 0f);

        _startPosition += _offsetPosition;
        _targetPosition -= _offsetPosition;
    }

    void OnUpdateWave(int _currentWave, bool _isStartWave)
    {
        StartCoroutine(WavesTextAnimate(_currentWave));
    }

    IEnumerator WavesTextAnimate(int _currentWave)
    {
        float _delay = 0.5f;
        yield return new WaitForSeconds(_delay);

        wavesAnimatedText.text = _patternWaveAnimatedText + _currentWave;
        _waveAnimatedRectTransfrom.anchoredPosition = _startPosition;

        float _duration = 2f;
        float _elapsed = 0f;
        float _speed;
        while (_elapsed <= _duration)
        {
            float _progress = _elapsed / _duration;

            if (_progress >= 0.45f && _progress <= 0.55f)
                _speed = 0.2f;
            else
                _speed = 1f;

            _elapsed += Time.deltaTime * _speed;

            _waveAnimatedRectTransfrom.anchoredPosition = Vector2.Lerp(_startPosition, _targetPosition, _progress);

            yield return null;
        }

        UpdateWaveText(_currentWave);
    }

    void UpdateWaveText(int _currentWave)
    {
        wavesText.text = _patternWaveText + _currentWave;
    }
}
