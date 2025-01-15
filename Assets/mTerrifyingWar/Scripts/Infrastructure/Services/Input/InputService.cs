using System;
using UnityEngine;

public class InputService : IInputService, IDisposable
{
    private PlayerInputActions _inputActions;

    public InputService()
    {
        _inputActions = new PlayerInputActions();
        
        _inputActions.Enable();
    }

    public Vector2 MoveDirection => _inputActions.Player.Move.ReadValue<Vector2>();
    public Vector2 LookDirection => _inputActions.Player.Look.ReadValue<Vector2>();
    public bool IsRun => _inputActions.Player.Run.IsPressed();
    public bool IsJump => _inputActions.Player.Jump.triggered;
    public bool IsCrouching => _inputActions.Player.Crouch.triggered;
    public bool IsShoot => _inputActions.Player.Shoot.IsPressed();
    public bool IsAim => _inputActions.Player.Aim.IsPressed();
    public bool IsReload => _inputActions.Player.Reload.IsPressed();
    public bool IsInteract => _inputActions.Player.Interact.IsPressed();

    public void Enable()
    {
        _inputActions.Enable();
    }
    
    public void Disable()
    {
        _inputActions.Disable();
    }

    public void Dispose()
    {
        _inputActions.Disable();
        _inputActions.Dispose();
    }
}