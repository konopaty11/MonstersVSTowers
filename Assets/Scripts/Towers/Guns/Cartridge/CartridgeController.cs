using UnityEngine;

public class CartridgeController : MonoBehaviour
{
    [SerializeField] GameObject particleSystemPrefab;
    [SerializeField] float particleSystemDelayDestroy;

    public RotatingAndShoutingGuns Gun { get; set; }
    public float Damage { get; private set; }

    string _monsterTag = "Monster";

    void Start()
    {
        Damage = Gun.GetLevelSettings().damage;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(_monsterTag))
        {
            MonsterController _monster = collision.collider.GetComponent<MonsterController>();
            _monster.CartridgeHit(Gun, Damage);
        }

        ContactPoint _contact = collision.contacts[0];
        GameObject _particleSystemObject = Instantiate
            (
                particleSystemPrefab, 
                _contact.point, 
                Quaternion.LookRotation(_contact.normal)
            );

        Destroy(_particleSystemObject, particleSystemDelayDestroy);
        Destroy(gameObject);

        ParticleSystemRenderer _psRenderer = _particleSystemObject.GetComponent<ParticleSystemRenderer>();
        _psRenderer.renderMode = ParticleSystemRenderMode.Mesh;
    }
}
