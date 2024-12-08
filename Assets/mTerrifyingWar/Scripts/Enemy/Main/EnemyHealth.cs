using System;
using UnityEngine;

[RequireComponent(typeof(EnemyAnimator))]
[RequireComponent(typeof(RagdollHandler))]
public class EnemyHealth : MonoBehaviour, IHealth
{
    [Space(5)]
    [SerializeField] private int _maxHealth;
    
    private int _currentHealth;

    public event Action EnemyDeath;
 
    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            EnemyDeath?.Invoke();
        }
    }
}