using System;
using R3;
using UnityEngine;

public class PlayerHealth : IHealth, IDisposable
{
    private readonly IGameplayFactory _gameplayFactory;
    private readonly IPersistentProgressService _persistentProgressService;
    
    private readonly ReactiveProperty<float> _health = new();
    public Observable<float> Health => _health;
    
    private GameState _gameState;
    private PlayerEntity _playerEntity;
    
    public float MaxHealth { get; private set; }
    
    public PlayerHealth(IPersistentProgressService persistentProgressService, IGameplayFactory gameplayFactory)
    {
        _gameplayFactory = gameplayFactory;
        _persistentProgressService = persistentProgressService;

        _gameplayFactory.CreatePlayerChanged += Setup;
    }

    private void Setup()
    {
        MaxHealth = _persistentProgressService.GameState.PlayerEntity.HealthEntity.MaxHealth;
        _health.Value = _persistentProgressService.GameState.PlayerEntity.HealthEntity.CurrentHealth;
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

    public void Dispose()
    {
        _gameplayFactory.CreatePlayerChanged -= Setup;
    }
}