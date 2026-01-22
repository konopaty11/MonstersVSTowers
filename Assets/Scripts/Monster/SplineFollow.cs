using UnityEngine;
using UnityEngine.Splines;

public class SplineFollow : MonoBehaviour
{
    public float Speed { get; set; }

    SplineContainer _container;
    bool _loop;

    float _currentDistance = 0f;
    public float NormalizePosition { get; private set; }
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

        NormalizePosition = _currentDistance / _splineLenght;
        if (_loop && NormalizePosition >= 1f)
            _currentDistance = 0f;

        transform.position = _container.EvaluatePosition(NormalizePosition);

        Vector3 _tangent = _container.EvaluateTangent(NormalizePosition);
        transform.rotation = Quaternion.LookRotation(_tangent);
    }
}
