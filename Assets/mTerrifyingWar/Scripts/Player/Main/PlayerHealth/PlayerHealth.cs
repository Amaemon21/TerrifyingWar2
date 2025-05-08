using R3;
using UnityEngine;

public class PlayerHealth : IHealth
{
    private readonly IStorageService _storageService;
    private readonly ReactiveProperty<float> _health;
    public Observable<float> Health => _health;
    
    private SaveData _saveData;
    
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
        
        _saveData.PlayerEntity.Health = _health.Value;
        _storageService.Save(_saveData);
    }

    public void Heal(float healAmount)
    {
        _health.Value = Mathf.Clamp(_health.Value + healAmount, 0, MaxHealth);
        
        _saveData.PlayerEntity.Health = _health.Value;
        _storageService.Save(_saveData);
    }

    private void LoadSaveData(SaveData saveData)
    {
        _saveData = saveData;

        MaxHealth = saveData.PlayerEntity.MaxHealth;
        _health.Value = saveData.PlayerEntity.Health;
    }
}