using System;
using UnityEngine;

public interface IInputService
{
    //Player
    public Vector2 MoveDirection {get;}
    public Vector2 LookDirection {get;}
    public bool IsRun {get;}
    public bool IsJump {get;}
    public bool IsCrouching {get;}
    public bool IsInteract {get;}
    public event Action OnShootStart;
    public event Action OnShootEnd;
    public bool IsAim {get;}
    public bool IsReload {get;}
    public bool ChangeFireMode {get;}
    public bool ThrowGrenade {get;}
    
    //UI
    public bool IsInventory { get; }
    public bool IsMap { get; }
    public bool IsEscape { get; }

    public void EnablePlayerMap();
    public void DisablePlayerMap();
}