public interface IDamageable
{
    public float CurrentHealth { get; }

    public void SubtractHealth(float _damage);
}
