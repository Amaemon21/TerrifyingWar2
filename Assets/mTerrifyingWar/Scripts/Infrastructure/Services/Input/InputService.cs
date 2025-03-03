using System;
using UnityEngine;

public class InputService : IInputService, IDisposable
{
    private readonly PlayerInputActions _inputActions;

    public InputService()
    {
        _inputActions = new PlayerInputActions();
        
        _inputActions.Enable();
    }

    //Player
    public Vector2 MoveDirection => _inputActions.Player.Move.ReadValue<Vector2>();
    public Vector2 LookDirection => _inputActions.Player.Look.ReadValue<Vector2>();
    public bool IsRun => _inputActions.Player.Run.IsPressed();
    public bool IsJump => _inputActions.Player.Jump.triggered;
    public bool IsCrouching => _inputActions.Player.Crouch.triggered;
    public bool IsShoot => _inputActions.Player.Shoot.IsPressed();
    public bool IsAim => _inputActions.Player.Aim.IsPressed();
    public bool IsReload => _inputActions.Player.Reload.IsPressed();
    public bool IsInteract => _inputActions.Player.Interact.IsPressed();
    
    //UI
    public bool IsInventory => _inputActions.UI.Inventory.triggered;
    public bool IsMap => _inputActions.UI.Map.triggered;
    public bool IsEscape => _inputActions.UI.Escap.triggered;
    
    public void EnablePlayerMap()
    {
        _inputActions.Player.Enable();
    }
    
    public void DisablePlayerMap()
    {
        _inputActions.Player.Disable();
    }

    public void Dispose()
    {
        _inputActions.Disable();
        _inputActions.Dispose();
    }
}