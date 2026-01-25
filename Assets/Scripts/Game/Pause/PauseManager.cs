using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void OnEnable()
    {
        LoadManager.OnLoad += ChangeButtons;
    }

    void OnDisable()
    {
        LoadManager.OnLoad -= ChangeButtons;
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
        Time.timeScale = 0f;
    }

    void ResumeGame()
    {
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
