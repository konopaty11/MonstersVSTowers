using System.Collections.Generic;
using UnityEngine;

public class CartridgePool : MonoBehaviour
{
    [SerializeField] GameObject cannonballPrefab;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] Transform cartridgesParent;

    Queue<GameObject> _cannonballs = new();
    Queue<GameObject> _arrows = new();

    int _cartridgesCount = 6;

    void Awake()
    {
        ServiceLocator.Register(this);
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        for (int i = 0; i < _cartridgesCount; i++)
        {
            GameObject _cannonball = Instantiate(cannonballPrefab, cartridgesParent);
            GameObject _arrow = Instantiate(arrowPrefab, cartridgesParent);

            _cannonballs.Enqueue(_cannonball);
            _arrows.Enqueue(_arrow);
        }
    }

    public GameObject InstantiateByPoll(GunType _gunType, Vector3 _position, Quaternion _rotation)
    {
        GameObject _cartridge = null;
        switch (_gunType)
        {
            case GunType.Cannon:
                _cartridge = GetCannonball();
                break;

            case GunType.Crossbow:
                _cartridge = GetArrow();
                break;
        }

        _cartridge.transform.position = _position;
        _cartridge.transform.rotation = _rotation;

        _cartridge.SetActive(true);

        return _cartridge;
    }

    public void ReturnCartridgeToPool(GunType _gunType, GameObject _cartridge)
    {
        _cartridge.transform.SetParent(cartridgesParent);

        Queue<GameObject> _cartridges = null;
        switch (_gunType)
        {
            case GunType.Cannon:
                _cartridges = _cannonballs;
                break;

            case GunType.Crossbow:
                _cartridges = _arrows;
                break;
        }

        if (_cartridges.Count == _cartridgesCount)
        {
            Destroy(_cartridge);
            return;
        }

        _cartridges.Enqueue(_cartridge);
        _cartridge.SetActive(false);
    }

    GameObject GetCannonball()
    {
        if (_cannonballs.Count == 0)
        {
            _cartridgesCount++;
            return Instantiate(cannonballPrefab, cartridgesParent);
        }

        return _cannonballs.Dequeue();
    }

    GameObject GetArrow()
    {
        if (_arrows.Count == 0)
        {
            _cartridgesCount++;
            return Instantiate(arrowPrefab, cartridgesParent);
        }

        return _cannonballs.Dequeue();
    }
}
