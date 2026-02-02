using System.Collections.Generic;
using UnityEngine;

public class CartridgePool : MonoBehaviour
{
    [SerializeField] GameObject cannonballPrefab;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] Transform cartridgesParent;

    Queue<GameObject> _cannonballs = new();
    Queue<GameObject> _arrows = new();

    int _cartridgesStartCount = 6;

    void Start()
    {
        Init();
    }

    void Init()
    {
        for (int i = 0; i < _cartridgesStartCount; i++)
        {
            GameObject _cannonball = Instantiate(cannonballPrefab, cartridgesParent);
            GameObject _arrow = Instantiate(cannonballPrefab, cartridgesParent);

            _cannonballs.Enqueue(_cannonball);
            _arrows.Enqueue(_arrow);
        }
    }

    public void InstantiateByPoll(GunType _gunType, Vector3 _position, Quaternion _rotation)
    {
        GameObject _cartridge = null;
        switch (_gunType)
        {
            case GunType.Cannon:
                _cartridge = _cannonballs.Dequeue();
                break;

            case GunType.Crossbow:
                _cartridge = _arrows.Dequeue();
                break;
        }

        _cartridge.transform.position = _position;
        _cartridge.transform.rotation = _rotation;
    }
}
