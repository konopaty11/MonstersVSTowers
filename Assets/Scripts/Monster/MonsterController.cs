using UnityEngine;
using UnityEngine.Splines;

public class MonsterController : MonoBehaviour
{
    [SerializeField] MonsterType type;
    [SerializeField] SplineAnimate spline;

    public MonsterType Type => type;

    public void InitMonster(SplineContainer _spline)
    {
        spline.Container = _spline;
        spline.Play();
    }

    public void SubstractHealth(float _damage)
    {

    }
}
