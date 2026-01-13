using UnityEngine;
using UnityEngine.UI;

public class CastleController : MonoBehaviour, IDamageable
{
    [SerializeField] Slider healthSlider;
    [SerializeField] GeneralSettings generalSettings;
    [SerializeField] GameManager gameManager;

    string _monsterTag = "Monster";
    float _damageCoefficient = 1.2f;

    public float CurrentHealth { get; private set; }

    void OnEnable()
    {
        GameManager.OnRestart += RestoreHealth;
    }

    void OnDisable()
    {
        GameManager.OnRestart -= RestoreHealth;
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        CurrentHealth = generalSettings.castleHealth;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(_monsterTag)) return;

        MonsterController _monster = other.GetComponent<MonsterController>();
        SubtractHealth(_monster.CurrentHealth * _damageCoefficient);

        if (CurrentHealth > 0)
            _monster.SubtractHealth(_monster.CurrentHealth);
    }

    public void SubtractHealth(float _damage)
    {
        healthSlider.gameObject.SetActive(true);

        CurrentHealth -= _damage;
        healthSlider.value = CurrentHealth / generalSettings.castleHealth;

        if (CurrentHealth <= 0)
            gameManager.Loose();
    }

    public void RestoreHealth()
    {
        CurrentHealth = generalSettings.castleHealth;
        healthSlider.value = 1f;
        healthSlider.gameObject.SetActive(false);
    }
}
