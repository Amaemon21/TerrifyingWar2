using UnityEngine;
using Zenject;

public class PlayerDeath : MonoBehaviour
{
    [Inject] private readonly PlayerController _controller;
    [Inject] private readonly IInputService _inputService;
    
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private GameObject _gameEndMenu;
    [SerializeField] private Rigidbody _deathEffect;
    
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
        _inputService.Disable();
    }

    private Rigidbody DeathEffectCreate()
    {
        var effect =  Instantiate(_deathEffect, transform.position, Quaternion.identity);
        
        effect.transform.rotation = Quaternion.Euler(0, 30, -20); 
        
        Vector3 hitForce = (Vector3.right + Vector3.up * 0.3f + Vector3.forward * 0.1f).normalized * 5f;
        effect.AddForce(hitForce, ForceMode.Impulse);
        
        Vector3 torque = new Vector3(0, 0, -20f); 
        effect.AddTorque(torque, ForceMode.Impulse);
        
        effect.angularDamping = 5f; 
        
        return effect;
    }
}