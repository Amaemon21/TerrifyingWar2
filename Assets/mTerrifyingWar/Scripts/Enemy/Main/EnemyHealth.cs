using System;
using UnityEngine;

[RequireComponent(typeof(EnemyAnimator))]
[RequireComponent(typeof(RagdollHandler))]
public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Space(5)]
    [SerializeField] private float _maxHealth;
    
    private float _currentHealth;
    private bool _isDeath;
    
    public event Action EnemyDeath;
 
    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            if (_isDeath == false)
            {
                EnemyDeath?.Invoke();
                _isDeath = true;
            }
        }
    }
}