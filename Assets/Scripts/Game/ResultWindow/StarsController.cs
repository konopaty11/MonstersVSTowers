using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsController : MonoBehaviour
{
    [SerializeField] List<RectTransform> stars;
    [SerializeField] List<AudioSource> audioSources;
    [SerializeField] VisibilityUIManager visibilityUIManager;

    string _starsID = "Stars";

    public void ShowStars(int _count)
    {
        StartCoroutine(VisibleStarsControl(Vector3.zero, Vector3.one, stars.GetRange(0, _count)));
    }

    IEnumerator VisibleStarsControl(Vector3 _startScale, Vector3 _targetScale, List<RectTransform> _stars)
    {
        visibilityUIManager.ShowUI(_starsID, ShowType.Fading);
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < _stars.Count; i++) 
        {
            yield return VisibleStarControl(_startScale, _targetScale, _stars[i]);
            audioSources[i].Play();

            yield return new WaitForSeconds(0.2f);
        }

        visibilityUIManager.HideUI(_starsID, ShowType.Fading);
    }

    IEnumerator VisibleStarControl(Vector3 _startScale, Vector3 _targetScale, RectTransform _star)
    {
        float _duration = 0.5f;

        _star.gameObject.SetActive(true);

        float _elapsed = 0f;
        while (_elapsed <= _duration)
        {
            _elapsed += Time.deltaTime;

            _star.localScale = Vector3.Slerp(_startScale, _targetScale, _elapsed / _duration);

            yield return null;
        }
    }
}
