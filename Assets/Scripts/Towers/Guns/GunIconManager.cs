using UnityEngine;

public class GunIconManager : MonoBehaviour
{
    [SerializeField] GameObject cannonIcon;
    [SerializeField] GameObject crossbowIcon;
    [SerializeField] GameObject magicCrystalIcon;

    bool _cannonIconActive;
    bool _crossbowIconActive;
    bool _magicCrystalIconActive;

    public void IconHandle(GunType _gunType, Vector3 _position)
    {
        switch (_gunType)
        {
            case GunType.Cannon:
                _cannonIconActive = true;
                cannonIcon.transform.position = _position;
                break;
            case GunType.Crossbow:
                _crossbowIconActive = true;
                crossbowIcon.transform.position = _position;
                break;
            case GunType.MagicCrystal:
                _magicCrystalIconActive = true;
                magicCrystalIcon.transform.position = _position;
                break;
        }

        cannonIcon.SetActive(_cannonIconActive);
        crossbowIcon.SetActive(_crossbowIconActive);
        magicCrystalIcon.SetActive(_magicCrystalIconActive);

        _cannonIconActive = false;
        _crossbowIconActive = false;
        _magicCrystalIconActive = false;
    }

    public void DisableIcons()
    {
        _cannonIconActive = false;
        _crossbowIconActive = false;
        _magicCrystalIconActive = false;

        cannonIcon.SetActive(_cannonIconActive);
        crossbowIcon.SetActive(_crossbowIconActive);
        magicCrystalIcon.SetActive(_magicCrystalIconActive);
    }
}
