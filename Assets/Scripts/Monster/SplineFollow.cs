using UnityEngine;
using UnityEngine.Splines;

public class SplineFollow : MonoBehaviour
{
    public float Speed { get; set; }

    SplineContainer _container;
    bool _loop;

    float _currentDistance = 0f;
    float _normalizePosition;
    float _splineLenght;

    bool _initialized;

    void Update()
    {
        Follow();
    }

    public void Init(SplineContainer _container, bool _loop)
    {
        this._container = _container;
        this._loop = _loop;

        _splineLenght = _container.CalculateLength();
        _initialized = true;
    }

    void Follow()
    {
        if (!_initialized) return;

        _currentDistance += Speed * Time.deltaTime;

        _normalizePosition = _currentDistance / _splineLenght;
        if (_loop && _normalizePosition >= 1f)
            _currentDistance = 0f;

        transform.position = _container.EvaluatePosition(_normalizePosition);

        Vector3 _tangent = _container.EvaluateTangent(_normalizePosition);
        transform.rotation = Quaternion.LookRotation(_tangent);
    }
}
