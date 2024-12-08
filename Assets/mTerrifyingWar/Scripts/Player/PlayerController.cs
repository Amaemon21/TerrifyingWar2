using System.Collections;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Inject] private readonly ShootTransform _cameraTransform;
    [Inject] private readonly InputService _inputService;
    
    [Space]
    [SerializeField] private float _speed = 10.0f;
    [SerializeField] private float _crouchSpeed = 5.0f;
    [SerializeField] private float _gravity = -20.0f;
    [SerializeField] private float _jumpHeight = 2.0f;
    [SerializeField] private float _junMultiplier = 2.0f;

    [Space]
    [SerializeField, Range(0f, 90f)] private float _jumpSlopeLimit = 0.0f;

    [Space]
    [SerializeField] private float _mouseSensitivity = 2.0f;

    [Header("Crouch")]
    [SerializeField] private float _crouchHeight = 0.5f;
    [SerializeField] private float _standingHeight = 2.0f;
    [SerializeField] private float _timeToCrouch = 0.25f;
    [SerializeField] private Vector3 _crouchingCenter = new(0.0f, 0.5f, 0.0f);
    [SerializeField] private Vector3 _standingCenter = new(0.0f, 0.0f, 0.0f);

    private CharacterController _characterController = null;
    private float _jumpMultiplier = 0.0f;
    private float _yVelocity = 0.0f;
    private float _originalSlopeLimit = 0.0f;
    private float _xRotation = 0.0f;
    private float _defaultSpeed = 0.0f;

    public bool IsCrouching { get; set; } = false;
    public bool DuringCrouchAnimation { get; set; } = false;
    public bool IsSprinting { get; set; } = false;
    public bool IsWalking { get; set; } = false;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        _originalSlopeLimit = _characterController.slopeLimit;
        _jumpMultiplier = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        _defaultSpeed = _speed;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        CheckGround();
        Move();
        CrouchCheck();
    }

    private void LateUpdate()
    {
        Look();
    }

    private void Look()
    {
        var mouseX = _inputService.LookDirection.x * _mouseSensitivity;
        var mouseY = _inputService.LookDirection.y * _mouseSensitivity;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        _cameraTransform.transform.localRotation = Quaternion.Euler(_xRotation, 0.0f, 0.0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void CrouchCheck()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && _characterController.isGrounded && !DuringCrouchAnimation)
        {
            Crouch();
        }
    }

    public void Crouch() 
    {
        if (IsCrouching)
        {
            _speed = _defaultSpeed;
        }
        else
        {
            _speed = _crouchSpeed;
        }

        StartCoroutine(CrouchHandle()); 
    }

    private void Move()
    {
        var x = _inputService.MoveDirection.x;
        var z = _inputService.MoveDirection.y;

        var move = (transform.right * x + transform.forward * z).normalized;
        move = _speed * Time.deltaTime * move;
        
        if (IsSprinting)
        {
            move *= _junMultiplier;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _characterController.isGrounded)
        {
            _yVelocity += _jumpMultiplier;
        }

        _yVelocity += _gravity * Time.deltaTime;

        move.y = _yVelocity * Time.deltaTime;

        _characterController.Move(move);

        if (x == 0 && z == 0)
        {
            IsWalking = false;
            IsSprinting = false;
        }
        else
        {
            IsWalking = true;

            if (IsWalking)
            {
                IsSprinting = Input.GetKey(KeyCode.LeftShift);
            }
            else
            {
                IsSprinting = false;
            }
        }
    }

    private void CheckGround()
    {
        if (_characterController.isGrounded || _characterController.collisionFlags == CollisionFlags.Above) _yVelocity = -0.1f;

        if (_characterController.isGrounded)
        {
            _characterController.slopeLimit = _originalSlopeLimit;
        }
        else
        {
            _characterController.slopeLimit = _jumpSlopeLimit;
        }
    }

    private IEnumerator CrouchHandle()
    {
        DuringCrouchAnimation = true;

        var timeElapsed = 0.0f;
        var targetHeight = IsCrouching ? _standingHeight : _crouchHeight;
        var currentHeight = _characterController.height;

        Vector3 targetCenter = IsCrouching ? _standingCenter : _crouchingCenter;
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
        IsCrouching = !IsCrouching;
        DuringCrouchAnimation = false;
    }
}