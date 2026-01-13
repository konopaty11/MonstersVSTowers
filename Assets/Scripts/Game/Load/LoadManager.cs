using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour
{
    [SerializeField] VisibilityUIManager visibilityUIManager;
    [SerializeField] GameObject gameCanvas;
    [SerializeField] GameObject menuCanvas;
    [SerializeField] GameObject loadCanvas;
    [SerializeField] new Transform camera;
    [SerializeField] Transform gameCameraTransform;
    [SerializeField] Transform menuCameraTransform;
    [SerializeField] float delayLoad;
    [SerializeField] List<Image> points;

    float _shiftElapsed = 0.2f;
    float _pointsAnimationDuration = 2f;

    string _loadWindowID = "Load";

    public void LoadGame() => LoadGame(null);

    public void LoadMenu() => LoadMenu(null);

    public void LoadGame(UnityAction _onComplete)
    {
        StartCoroutine(LoadControl(true, false, gameCameraTransform, _onComplete));
    }

    public void LoadMenu(UnityAction _onComplete)
    {
        StartCoroutine(LoadControl(false, true, menuCameraTransform, _onComplete));
    }

    //IEnumerator PointsAnimation()
    //{
    //    float _elapsed 
    //}

    IEnumerator LoadControl(bool _gameCanvasActive, bool _menuCanvasActive, Transform _cameraTransform, UnityAction _onComplete = null)
    {
        loadCanvas.SetActive(true);
        visibilityUIManager.ShowUI(_loadWindowID, ShowType.Moving);

        yield return new WaitForSeconds(delayLoad);

        gameCanvas.SetActive(_gameCanvasActive);
        menuCanvas.SetActive(_menuCanvasActive);

        camera.position = _cameraTransform.position;

        visibilityUIManager.HideUI(_loadWindowID, ShowType.Moving);
        yield return new WaitForSeconds(delayLoad);
        loadCanvas.SetActive(false);

        if (_onComplete != null)
            _onComplete();
    }
}
