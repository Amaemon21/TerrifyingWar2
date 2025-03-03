using UnityEngine;

public interface IInputService
{
    public Vector2 MoveDirection {get;}
    public Vector2 LookDirection {get;}
    public bool IsRun {get;}
    public bool IsJump {get;}
    public bool IsCrouching {get;}
    public bool IsShoot {get;}
    public bool IsAim {get;}
    public bool IsReload {get;}
    public bool IsInteract {get;}
    
    public bool IsInventory { get; }
    public bool IsMap { get; }
    public bool IsEscape { get; }

    public void EnablePlayerMap();
    public void DisablePlayerMap();
}