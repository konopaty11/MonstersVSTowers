using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] VisibilityUIManager visibilityUIManager;
    [SerializeField] SoundManager soundManager;
    [SerializeField] List<AudioSource> audioSources;
    [SerializeField] GameManager gameManager;

    string _pauseID = "Pause";
    float _duration = 0.5f;


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
        gameManager.LoadMenu();
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
