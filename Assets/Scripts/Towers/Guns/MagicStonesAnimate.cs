using UnityEngine;

public class MagicStonesAnimate : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] float movementIntensity;
    [SerializeField] float movementSpeed;

    MeshFilter _meshFilter;

    float _time;

    void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
    }

    void Update()
    {
        Animation();
    }

    void Animation()
    {
        if (_meshFilter.mesh == null) return;

        Rotate();
        Move();
    }

    void Rotate()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.World);
    }

    void Move()
    {
        _time += Time.deltaTime * movementSpeed;
        float _y = Mathf.Sin(_time) * movementIntensity;

        transform.position = new(transform.position.x, transform.position.y + _y, transform.position.z);
    }
}
