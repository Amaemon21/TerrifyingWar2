using System;
using System.Collections;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina Settings")]
    [SerializeField] private float _maxStamina = 100;
    [SerializeField] private float _sprintStaminaCost = 10;
    [SerializeField] private float _jumpStaminaCost = 20;
    [SerializeField] private float _staminaRecoveryDelay = 2.0f;
    [SerializeField] private float _staminaRecoveryRate = 5;

    private float _currentStamina;
    private PlayerController _playerController;
    private Coroutine _recoveryCoroutine;

    public event Action<float, float> StaminaChanged;

    private void Awake()
    {
        _currentStamina = _maxStamina;
        
        _playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        HandleStaminaUsage();
    }

    private void HandleStaminaUsage()
    {
        if (_playerController.IsSprinting && _currentStamina > 0)
        {
            ReduceStamina(_sprintStaminaCost);
        }
        else if (!_playerController.IsSprinting && _currentStamina < _maxStamina && _recoveryCoroutine == null)
        {
            _recoveryCoroutine = StartCoroutine(RecoverStamina());
        }

        if (_currentStamina <= 0 && _playerController.IsSprinting)
        {
            _playerController.IsSprinting = false;
        }
    }

    public void UseStaminaForJump()
    {
        if (_currentStamina >= _jumpStaminaCost)
        {
            ReduceStamina(_jumpStaminaCost);
        }
    }

    private void ReduceStamina(float amount)
    {
        _currentStamina = Mathf.Max(_currentStamina - amount, 0);
        StaminaChanged?.Invoke(_currentStamina, _maxStamina);

        if (_recoveryCoroutine != null)
        {
            StopCoroutine(_recoveryCoroutine);
            _recoveryCoroutine = null;
        }
    }

    private IEnumerator RecoverStamina()
    {
        yield return new WaitForSeconds(_staminaRecoveryDelay);
        
        while (_currentStamina < _maxStamina)
        {
            _currentStamina = Mathf.MoveTowards(_currentStamina, _maxStamina, _staminaRecoveryRate * Time.deltaTime);
            StaminaChanged?.Invoke(_currentStamina, _maxStamina);
            yield return null;
        }

        _recoveryCoroutine = null;
    }
}