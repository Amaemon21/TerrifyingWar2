using UnityEngine;

public class HealthBar : Bar
{
    [SerializeField] private PlayerHealth _playerHealth;

    protected override void OnEnable()
    {
        _playerHealth.HealthChanged += OnBarChanged;
    }

    protected override void OnDisable()
    {
        _playerHealth.HealthChanged -= OnBarChanged;
    }
}