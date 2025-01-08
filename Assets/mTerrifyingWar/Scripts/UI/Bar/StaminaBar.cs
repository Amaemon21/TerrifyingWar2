using UnityEngine;

public class StaminaBar : Bar
{
    [SerializeField] private PlayerStamina _playerStamina;

    protected override void OnEnable()
    {
        _playerStamina.StaminaChanged += OnBarChanged;
    }

    protected override void OnDisable()
    {
        _playerStamina.StaminaChanged -= OnBarChanged;
    }
}