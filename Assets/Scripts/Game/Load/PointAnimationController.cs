using System.Collections;
using UnityEngine;

public class PointAnimationController : MonoBehaviour
{
    [SerializeField] float startElapsed;
    [SerializeField] Color targetColor;

    float _duration = 2f;

    void Start()
    {
        
    }

    IEnumerator PointAnimation()
    {
        float _elapsed = startElapsed;
        while (true)
        {
            _elapsed += Time.deltaTime;

            yield return null;
        }
    }
}
