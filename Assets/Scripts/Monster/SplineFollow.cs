using UnityEngine;
using UnityEngine.Splines;

public class SplineFollow : MonoBehaviour
{
    public SplineContainer Container { get; set; }
    public float Speed { get; set; }
    public bool Loop { get; set; }

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
        if (Loop && _normalizePosition >= 1f)
            _currentDistance = 0f;

        transform.position = Container.EvaluatePosition(_normalizePosition);

        Vector3 _tangent = Container.EvaluateTangent(_normalizePosition);
        transform.rotation = Quaternion.LookRotation(_tangent);
    }
}
