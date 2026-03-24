using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [SerializeField] VisibilityUIManager visibilityUIManager;
    [SerializeField] SoundManager soundManager;
    [SerializeField] List<AudioSource> audioSources;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject menuButtons;
    [SerializeField] GameObject gameButtons;

    string _pauseID = "Pause";
    float _duration = 0.5f;

    public bool IsPause { get; private set; }

    InputSystem_Actions _inputSystem;

    float _doubleTapTime = 0.3f;
    float _currentTime = 0f;
    bool _isTimeActive = true;

    void Awake()
    {
        _inputSystem = new();
    }

    void OnEnable()
    {
        LoadManager.OnLoad += ChangeButtons;
    }

    void OnDisable()
    {
        LoadManager.OnLoad -= ChangeButtons;
    }

    void Update()
    {
        Timer();
    }

    void Timer()
    {
        if (_isTimeActive)
            _currentTime += Time.deltaTime;
    }

    void ChangeButtons(LocationType _type)
    {
        bool _isMenuLocation = _type == LocationType.Menu;

        menuButtons.SetActive(_isMenuLocation);
        gameButtons.SetActive(!_isMenuLocation);
    }

    public void OpenPause()
    {
        visibilityUIManager.ShowUI(_pauseID, ShowType.Moving);
        StartCoroutine(PauseSound());
    }

    IEnumerator PauseSound()
    {
        soundManager.FadeActiveAudioSourceToPause(_duration);

        yield return new WaitForSeconds(_duration);
        PauseGame();
    }

    void PauseGame()
    {
        IsPause = true;
        Time.timeScale = 0f;
    }

    void ResumeGame()
    {
        IsPause = false;
        Time.timeScale = 1f;
    }

    public void ToMenu()
    {
        visibilityUIManager.HideUI(_pauseID, ShowType.Moving);
        ResumeGame();
        gameManager.LoadSaveMenu();
    }

    public void ClosePause()
    {
        visibilityUIManager.HideUI(_pauseID, ShowType.Moving);
        ResumeGame();
        ResumeSound();
    }

    void ResumeSound()
    {
        soundManager.FadeActiveAudioSourceToResume(_duration);
    }

}
