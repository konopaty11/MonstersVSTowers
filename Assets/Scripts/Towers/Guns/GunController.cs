using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] GunType type;

    public GunType Type => type;
}
