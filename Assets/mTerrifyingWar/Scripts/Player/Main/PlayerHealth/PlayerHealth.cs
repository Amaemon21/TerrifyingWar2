using R3;
using UnityEngine;

public class PlayerHealth : IHealth
{
    private readonly IPersistentProgressService _persistentProgressService;
    private readonly ReactiveProperty<float> _health;
    public Observable<float> Health => _health;
    
    private GameState _gameState;
    private PlayerEntity _playerEntity;
    
    public float MaxHealth { get; private set; }
    
    public PlayerHealth(IPersistentProgressService persistentProgressService)
    {
        _persistentProgressService = persistentProgressService;
        
        MaxHealth = _persistentProgressService.GameState.PlayerEntity.HealthEntity.MaxHealth;
        _health = new ReactiveProperty<float>(_persistentProgressService.GameState.PlayerEntity.HealthEntity.CurrentHealth);
    }
    
    public void TakeDamage(float damage)
    {
        _health.Value = Mathf.Clamp(_health.Value - damage, 0, MaxHealth);
        
        _playerEntity.HealthEntity.CurrentHealth = _health.Value;
        _persistentProgressService.GameState.PlayerEntity.HealthEntity.CurrentHealth = _health.Value;
    }

    public void Heal(float healAmount)
    {
        _health.Value = Mathf.Clamp(_health.Value + healAmount, 0, MaxHealth);
        
        _playerEntity.HealthEntity.CurrentHealth = _health.Value;
        _persistentProgressService.GameState.PlayerEntity.HealthEntity.CurrentHealth = _health.Value;
    }
}