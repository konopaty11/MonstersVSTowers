using UnityEngine;
using UnityEngine.Splines;

public class SplineFollow : MonoBehaviour
{
    [SerializeField] bool loop;

    public SplineContainer Container { get; set; }
    public float Speed { get; set; }

    float _currentDistance = 0f;
    float _normalizePosition;
    float _splineLenght;

    private void Start()
    {
        _splineLenght = Container.CalculateLength();
    }

    void Update()
    {
        Follow();
    }

    void Follow()
    {
        _currentDistance += Speed * Time.deltaTime;

        _normalizePosition = _currentDistance / _splineLenght;
        if (loop && _normalizePosition >= 1f)
            _normalizePosition = 0f;

        transform.position = Container.EvaluatePosition(_normalizePosition);

        Vector3 _tangent = Container.EvaluateTangent(_normalizePosition);
        transform.rotation = Quaternion.LookRotation(_tangent);
    }
}
