using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputService : IInputService, IDisposable
{
    private readonly PlayerInputActions _inputActions;

    public InputService()
    {
        _inputActions = new PlayerInputActions();
        
        _inputActions.Enable();

        _inputActions.Player.Shoot.started += OnShootStarted;
        _inputActions.Player.Shoot.canceled += OnShootCanceled;
    }

    //Player
    public Vector2 MoveDirection => _inputActions.Player.Move.ReadValue<Vector2>();
    public Vector2 LookDirection => _inputActions.Player.Look.ReadValue<Vector2>();
    public bool IsRun => _inputActions.Player.Run.IsPressed();
    public bool IsJump => _inputActions.Player.Jump.WasPressedThisFrame();
    public bool IsCrouching => _inputActions.Player.Crouch.WasPressedThisFrame();
    public bool IsAim => _inputActions.Player.Aim.IsPressed();
    public bool IsReload => _inputActions.Player.Reload.IsPressed();
    public bool ChangeFireMode => _inputActions.Player.ChangeFireMode.WasPressedThisFrame();
    public bool ThrowGrenade => _inputActions.Player.ThrowGrenade.WasPressedThisFrame();
    public bool IsInteract => _inputActions.Player.Interact.WasPressedThisFrame();
    
    public event Action OnShootStart;
    public event Action OnShootEnd;
    
    private void OnShootStarted(InputAction.CallbackContext context) => OnShootStart?.Invoke();
    private void OnShootCanceled(InputAction.CallbackContext context) => OnShootEnd?.Invoke();
    
    //UI
    public bool IsInventory => _inputActions.UI.Inventory.WasPressedThisFrame();
    public bool IsMap => _inputActions.UI.Map.WasPressedThisFrame();
    public bool IsEscape => _inputActions.UI.Escap.WasPressedThisFrame();
    
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
        _inputActions.Player.Shoot.started -= OnShootStarted;
        _inputActions.Player.Shoot.canceled -= OnShootCanceled;
        
        _inputActions.Disable();
        _inputActions.Dispose();
    }
}