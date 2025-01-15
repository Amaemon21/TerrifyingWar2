using UnityEngine;
using Zenject;

public class PlayerDeath : MonoBehaviour
{
    [Inject] private readonly IInputService _inputService;
    [Inject] private readonly UIWindowService _windowService;
    
    [SerializeField] private PlayerHealth _playerHealth;
    
    private bool _isDead = false;

    private void OnEnable()
    {
        _playerHealth.PlayerDeathChanged += Die;
    }

    private void OnDisable()
    {
        _playerHealth.PlayerDeathChanged -= Die;
    }

    private void Die()
    {
        if (_isDead) 
            return;

        _isDead = true;

        _windowService.OpenWindow(WindowType.GameEnd);
    }
}