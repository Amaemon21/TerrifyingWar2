using R3;
using UnityEngine;

public class PlayerHealth : IHealth
{
    private readonly IStorageService _storageService;
    private readonly ReactiveProperty<float> _health;
    public Observable<float> Health => _health;
    
    private GameState _gameState;
    private PlayerEntity _playerEntity;
    
    public float MaxHealth { get; private set; }
    
    public PlayerHealth(IStorageService storageService)
    {
        _health = new ReactiveProperty<float>();
        
        _storageService = storageService;
        
        _storageService.Load(LoadSaveData);
    }
    
    public void TakeDamage(float damage)
    {
        _health.Value = Mathf.Clamp(_health.Value - damage, 0, MaxHealth);
        
        _playerEntity.Health = _health.Value;
        _storageService.Save(_gameState);
    }

    public void Heal(float healAmount)
    {
        _health.Value = Mathf.Clamp(_health.Value + healAmount, 0, MaxHealth);
        
        _playerEntity.Health = _health.Value;
        _storageService.Save(_gameState);
    }

    private void LoadSaveData(GameState gameState)
    {
        _gameState = gameState;

        foreach (var entity in _gameState.Entities)
        {
            if (entity is PlayerEntity playerEntity)
            {
                _playerEntity = playerEntity;
            }
        }
        
        MaxHealth = _playerEntity.MaxHealth;
        _health.Value = _playerEntity.Health;
    }
}