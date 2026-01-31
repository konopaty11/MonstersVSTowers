using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] new Transform camera;
    [SerializeField] Transform startTransform;
    [SerializeField] Vector3 offsetPosition;
    [SerializeField] Vector3 offsetRotation;
    [SerializeField] float duration;

    public void GoToTower(Transform _towerTransform)
    {
        Vector3 _targetPosition = _towerTransform.position + offsetPosition;
        StartCoroutine(MovementHandle(camera.position, _targetPosition, duration));

        Quaternion _targetRotation = Quaternion.LookRotation(offsetRotation - offsetPosition);
        StartCoroutine(RotationHandle(camera.rotation, _targetRotation, duration));
    }

    public void GoToStartPosition()
    {
        StartCoroutine(MovementHandle(camera.position, startTransform.position, duration));
        StartCoroutine(RotationHandle(camera.rotation, startTransform.rotation, duration));
    }

    IEnumerator MovementHandle(Vector3 _startPosition, Vector3 _targetPosition, float _duration)
    {
        float _elapsed = 0f;
        while (_elapsed <= _duration)
        {
            _elapsed += Time.deltaTime;

            camera.position = Vector3.Slerp(_startPosition, _targetPosition, _elapsed / _duration);

            yield return null;
        }
    }

    IEnumerator RotationHandle(Quaternion _startRotation, Quaternion _targetRotation, float _duration)
    {
        float _elapsed = 0f;
        while (_elapsed <= _duration)
        {
            _elapsed += Time.deltaTime;

            camera.rotation = Quaternion.Slerp(_startRotation, _targetRotation, _elapsed / _duration);

            yield return null;
        }
    }
}
