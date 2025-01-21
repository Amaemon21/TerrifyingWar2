using R3;
using UnityEngine;

public class PlayerHealth
{
    private readonly ReactiveProperty<float> _health;
    public Observable<float> Health => _health;
    public float MaxHealth { get; private set; }
    
    public PlayerHealth()
    {
        MaxHealth = 100;
        _health = new ReactiveProperty<float>(MaxHealth);
    }
    
    public void TakeDamage(float damage)
    {
        _health.Value = Mathf.Clamp(_health.Value - damage, 0, MaxHealth);
    }

    public void Heal(float healAmount)
    {
        _health.Value = Mathf.Clamp(_health.Value + healAmount, 0, MaxHealth);
    }
}