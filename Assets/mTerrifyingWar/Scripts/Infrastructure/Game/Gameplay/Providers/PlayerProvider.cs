using UnityEngine;

public class PlayerProvider
{
    public PlayerMover PlayerMover { get; private set; }
    public Camera MainCamera { get; private set; }
    public PlayerController PlayerController { get; private set; }
    public WeaponContainer WeaponContainer { get; private set; }
    
    public void Setup(PlayerContainer playerContainer)
    {
        PlayerMover = playerContainer.PlayerMover;
        MainCamera = playerContainer.MainCamera;
        PlayerController = playerContainer.PlayerController;
        WeaponContainer = playerContainer.WeaponContainer;
    }
}