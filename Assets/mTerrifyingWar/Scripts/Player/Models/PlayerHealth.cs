using R3;
using UnityEngine;

public class PlayerHealth : IHealth
{
    private readonly ReactiveProperty<float> _health;
    public Observable<float> Health => _health;
    public float MaxHealth { get; private set; }
    
    public PlayerHealth(PlayerSettingsConfig playerSettingsConfig)
    {
        MaxHealth = playerSettingsConfig.MaxHealth;
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