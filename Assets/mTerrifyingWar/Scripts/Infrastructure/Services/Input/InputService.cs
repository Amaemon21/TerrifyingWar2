using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputService : MonoBehaviour
{
    [field: SerializeField] public PlayerInput PlayerInput { get; private set; }
    
    public event Action AimChanged; 
    public event Action ReloadChanged; 

    public Vector2 MoveDirection { get; private set; }
    public Vector2 LookDirection { get; private set; }
    public bool IsRun { get; private set; }
    public bool IsJump { get; private set; }
    public bool IsShoot { get; private set; }
    public bool IsAim { get; private set; }
    public bool IsReload { get; private set; }

    private void OnMove(InputValue value)
    {
        MoveDirection = value.Get<Vector2>();
    }

    private void OnLook(InputValue value)
    {
        LookDirection = value.Get<Vector2>();
    }

    private void OnRun(InputValue value)
    {
        IsRun = value.isPressed;
    }
    
    private void OnJump(InputValue value)
    {
        IsJump = value.isPressed;
    }

    private void OnShoot(InputValue value)
    {
        IsShoot = value.isPressed;
    }

    public void ResetShoot()
    {
        IsShoot = false;
    }

    private void OnAim(InputValue value)
    {
        IsAim = value.isPressed;
        AimChanged?.Invoke();
    }

    private void OnReload(InputValue value)
    {
        IsReload = value.isPressed;
        ReloadChanged?.Invoke();
    }
}