using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource mainAudioSource;
    [SerializeField] AudioSource menuAudioSource;
    [SerializeField] Slider volume;
    [SerializeField] Saves saves;
    [SerializeField] VisibilityUIManager visibleUIManager;

    float _durationFade = 2f;
    float _volume = 1f;
    public float Volume
    {
        get => _volume;
        set 
        {
            _volume = value;
            if (_isGameMusic)
                mainAudioSource.volume = _volume;
            else
                menuAudioSource.volume = _volume;
        }
    }

    string _soundWindowID = "Sound";

    float _prefVolume;

    bool _isGameMusic = false;

    void OnEnable()
    {
        Saves.OnDataLoaded += OnLoadData;
    }

    void OnDisable()
    {
        Saves.OnDataLoaded -= OnLoadData;
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        volume.onValueChanged.AddListener(OnSliderValueChanged);

        mainAudioSource.Play();
        mainAudioSource.Pause();
        menuAudioSource.Play();
    }

    void OnLoadData(SaveData _saveData)
    {
        Volume = _saveData.volume;
        volume.value = Volume;
    }

    public void ToMenuMusic()
    {
        _isGameMusic = false;
        StartCoroutine(FadeToPause(mainAudioSource, _durationFade));
        StartCoroutine(FadeToResume(menuAudioSource, _durationFade));
    }

    public void ToMainMusic()
    {
        _isGameMusic = true;
        StartCoroutine(FadeToPause(menuAudioSource, _durationFade));
        StartCoroutine(FadeToResume(mainAudioSource, _durationFade));
    }

    public void OpenSoundWindow()
    {
        _prefVolume = _volume;
        visibleUIManager.ShowUI(_soundWindowID, ShowType.Moving);
    }

    public void CloseSoundWindow()
    { 
        visibleUIManager.HideUI(_soundWindowID, ShowType.Moving);
    }

    public void CloseSoundWindowWithoutSave()
    {
        Volume = _prefVolume;
        volume.value = _volume;
        
        saves.SetVolume(_volume);
        CloseSoundWindow();
    }

    void OnSliderValueChanged(float _value)
    {
        Volume = _value;

        saves.SetVolume(_volume);
    }

    public void SaveVolume()
    {
        saves.SetVolume(_volume);
        CloseSoundWindow();
    }

    public void FadeActiveAudioSourceToPause(float _duration)
    {
        AudioSource _audioSource = _isGameMusic ? mainAudioSource : menuAudioSource;
        StartCoroutine(FadeToPause(_audioSource, _duration));
    }

    public void FadeActiveAudioSourceToResume(float _duration)
    {
        AudioSource _audioSource = _isGameMusic ? mainAudioSource : menuAudioSource;
        StartCoroutine(FadeToResume(_audioSource, _duration));
    }

    IEnumerator FadeToPause(AudioSource _audioSource, float _duration)
    {
        float _startVolume = _volume;
        _audioSource.volume = _startVolume;
        float _targetVolume = 0f;
        float _elapsed = 0f;
        while (_elapsed <= _duration)
        {
            _elapsed += Time.deltaTime;

            _audioSource.volume = Mathf.Lerp(_startVolume, _targetVolume, _elapsed / _duration);

            yield return null;
        }

        _audioSource.Pause();
    }

    IEnumerator FadeToResume(AudioSource _audioSource, float _duration)
    {
        _audioSource.UnPause();

        float _startVolume = 0f;
        _audioSource.volume = _startVolume;
        float _targetVolume = _volume;
        float _elapsed = 0f;
        while (_elapsed <= _duration)
        {
            _elapsed += Time.deltaTime;

            _audioSource.volume = Mathf.Lerp(_startVolume, _targetVolume, _elapsed / _duration);

            yield return null;
        }
    }
}
