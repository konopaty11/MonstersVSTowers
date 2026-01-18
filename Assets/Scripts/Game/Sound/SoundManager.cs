using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource mainAudioSource;
    [SerializeField] AudioSource menuAudioSource;

    float _duration = 2f;

    void Start()
    {
        Init();
    }

    void Init()
    {
        mainAudioSource.Play();
        mainAudioSource.Pause();
        menuAudioSource.Play();
    }

    public void ToMenuMusic()
    {
        StartCoroutine(FadeToPause(mainAudioSource));
        StartCoroutine(FadeToResume(menuAudioSource));
    }

    public void ToMainMusic()
    {
        StartCoroutine(FadeToPause(menuAudioSource));
        StartCoroutine(FadeToResume(mainAudioSource));
    }

    IEnumerator FadeToPause(AudioSource _audioSource)
    {
        float _startVolume = 1f;
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

    IEnumerator FadeToResume(AudioSource _audioSource)
    {
        _audioSource.UnPause();

        float _startVolume = 0f;
        _audioSource.volume = _startVolume;
        float _targetVolume = 1f;
        float _elapsed = 0f;
        while (_elapsed <= _duration)
        {
            _elapsed += Time.deltaTime;

            _audioSource.volume = Mathf.Lerp(_startVolume, _targetVolume, _elapsed / _duration);

            yield return null;
        }
    }
}
