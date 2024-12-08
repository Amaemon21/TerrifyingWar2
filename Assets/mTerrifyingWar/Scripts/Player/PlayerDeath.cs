using UnityEngine;
using Zenject;

public class PlayerDeath : MonoBehaviour
{
    [Inject] private readonly PlayerController _controller;
    [Inject] private readonly InputService _inputService;
    
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private GameObject _gameEndMenu;
    
    private CharacterController _characterController;
    
    private bool isDead = false;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        
        _gameEndMenu.SetActive(false);
    }

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
        if (isDead) 
            return;

        isDead = true;
        
        _controller.enabled = false;
        _characterController.enabled = false;
        _inputService.PlayerInput.enabled = false;
        
        _gameEndMenu.SetActive(true);
        
    }
}