using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField] MonsterType type;

    public MonsterType Type => type;
}
