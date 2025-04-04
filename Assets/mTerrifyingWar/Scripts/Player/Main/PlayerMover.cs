using System.Collections;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CharacterController))]
public class PlayerMover : MonoBehaviour
{
    [Inject] private readonly IInputService _inputService;
    
    [Space]
    [SerializeField] private float _speed = 4f;
    [SerializeField] private float _crouchSpeed = 5.0f;
    [SerializeField] private float _gravity = -20.0f;
    [SerializeField] private float _jumpHeight = 2.0f;
    [SerializeField] private float _runMultiplier = 2.0f;

    [Space]
    [SerializeField, Range(0f, 90f)] private float _jumpSlopeLimit = 0.0f;
    
    [Header("Crouch")]
    [SerializeField] private float _crouchHeight = 0.5f;
    [SerializeField] private float _standingHeight = 2.0f;
    [SerializeField] private float _timeToCrouch = 0.25f;
    [SerializeField] private Vector3 _crouchingCenter = new(0.0f, 0.5f, 0.0f);
    [SerializeField] private Vector3 _standingCenter = new(0.0f, 0.0f, 0.0f);
    
    private Transform _transform;
    private Animator _animator;
    private CharacterController _characterController = null;
    private float _jumpMultiplier = 0.0f;
    private float _yVelocity = 0.0f;
    private float _originalSlopeLimit = 0.0f;
    private float _defaultSpeed = 0.0f;
    private float _velocitySmoothDamp;
    private bool _duringCrouchAnimation = false;
    
    private void Awake()
    {
        _transform = transform;
        
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        
        _originalSlopeLimit = _characterController.slopeLimit;
        _jumpMultiplier = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        _defaultSpeed = _speed;
    }

    private void Update()
    {
        CheckGround();
        Move();
        CrouchCheck();
    }
    
    private void Move()
    {
        Vector2 moveDirection = new Vector2(_inputService.MoveDirection.x, _inputService.MoveDirection.y);

        Vector3 move = (_transform.right * moveDirection.x + _transform.forward * moveDirection.y).normalized;
        move = _speed * Time.deltaTime * move;
        
        if (_inputService.IsRun && !_inputService.IsCrouching)
        {
            move *= _runMultiplier;
        }

        Jump();
        
        _yVelocity += _gravity * Time.deltaTime;

        move.y = _yVelocity * Time.deltaTime;

        _characterController.Move(move);
    }

    private void Jump()
    {
        if (_inputService.IsJump && _characterController.isGrounded)
        {
            _yVelocity += _jumpMultiplier;
            _animator.SetBool(AnimationsConstrains.IS_IN_AIR, true);
            StartCoroutine(LandAfterDelay(0.4f));
        }
    }

    private IEnumerator LandAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnLand();
    }
    
    private void OnLand()
    {
        _animator.SetBool(AnimationsConstrains.IS_IN_AIR, false);
    }

    private void CheckGround()
    {
        if (_characterController.isGrounded || _characterController.collisionFlags == CollisionFlags.Above) 
            _yVelocity = -0.1f;

        _characterController.slopeLimit = _characterController.isGrounded ? _originalSlopeLimit : _jumpSlopeLimit;
    }

    private void CrouchCheck()
    {
        if (_inputService.IsCrouching && _characterController.isGrounded && !_duringCrouchAnimation)
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        _speed = _inputService.IsCrouching ? _defaultSpeed : _crouchSpeed;

        StartCoroutine(CrouchHandle());
    }
    
    private IEnumerator CrouchHandle()
    {
        _duringCrouchAnimation = true;

        float timeElapsed = 0.0f;
        float targetHeight = _inputService.IsCrouching ? _standingHeight : _crouchHeight;
        float currentHeight = _characterController.height;

        Vector3 targetCenter = _inputService.IsCrouching ? _standingCenter : _crouchingCenter;
        Vector3 currentCenter = _characterController.center;


        while (timeElapsed < _timeToCrouch)
        {
            _characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / _timeToCrouch);
            _characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / _timeToCrouch);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        _characterController.height = targetHeight;
        _characterController.center = targetCenter;
        _duringCrouchAnimation = false;
    }
}