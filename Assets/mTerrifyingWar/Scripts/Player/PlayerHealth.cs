using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [SerializeField] private int _maxHealth;

    private int _currentHealth;

    public event Action<int, int> HealthChanged;
    public event Action PlayerDeathChanged;
    
    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (damage >= 0)
        {
            _currentHealth -= damage;
        }

        if (_currentHealth <= 0)
        {
            PlayerDeathChanged?.Invoke();
        }
        
        HealthChanged?.Invoke(_currentHealth, _maxHealth);
    }
}