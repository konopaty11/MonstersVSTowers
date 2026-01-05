using UnityEngine;

public class ArrowFlyingController : MonoBehaviour
{
    [SerializeField] Rigidbody rg;
    [SerializeField] float force = 10f;
    [SerializeField] float offsetForward = 2f;

    void Start()
    {
        rg.AddForceAtPosition(Vector3.down * force, rg.transform.position + rg.transform.forward * offsetForward);
    }
}
