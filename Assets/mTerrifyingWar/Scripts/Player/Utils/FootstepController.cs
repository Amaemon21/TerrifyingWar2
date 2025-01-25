using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FootstepController : MonoBehaviour
{
    [Inject] private readonly IInputService _inputService;
    
    [Header("Settings")]
    [SerializeField] private AudioSource _footstepAudioSource; 
    [SerializeField] private List<AudioClip> _footstepSounds; 
    
    [Header("Player Settings")]
    [SerializeField] private float _baseStepInterval = 0.5f; 

    private float _stepTimer;
    private CharacterController _characterController;
    private PlayerController _playerController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (_inputService.MoveDirection.sqrMagnitude <= 0f)
        {
            _footstepAudioSource.Stop();
            return;
        }
        
        if (_characterController.isGrounded && _inputService.MoveDirection.sqrMagnitude > 0.1f && !_playerController.IsCrouching)
        {
            float speedFactor = _playerController.IsSprinting ? 0.5f : 1.0f; 
            float stepInterval = _baseStepInterval * speedFactor;

            _stepTimer += Time.deltaTime;

            if (_stepTimer >= stepInterval)
            {
                PlayFootstepSound();
                _stepTimer = 0f;
            }
        }
        else
        {
            _stepTimer = 0f;
        }
    }

    private void PlayFootstepSound()
    {
        if (_footstepSounds != null && _footstepSounds.Count > 0)
        {
            int index = Random.Range(0, _footstepSounds.Count);
            _footstepAudioSource.clip = _footstepSounds[index];
            _footstepAudioSource.Play();
        }
    }
}