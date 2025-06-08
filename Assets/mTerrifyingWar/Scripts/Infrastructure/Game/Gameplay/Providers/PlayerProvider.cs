using UnityEngine;

public class PlayerProvider
{
    public PlayerMover PlayerMover { get; private set; }
    public Camera MainCamera { get; private set; }
    public DropPosition DropPosition { get; private set; }
    public GameObject CinematicCamera { get; private set; }
    public PlayerController PlayerController { get; private set; }
    public WeaponContainer WeaponContainer { get; private set; }
    public UIBluer UIBluer { get; private set; }
    
    public void Setup(PlayerContainer playerContainer)
    {
        PlayerMover = playerContainer.PlayerMover;
        MainCamera = playerContainer.MainCamera;
        DropPosition = playerContainer.DropPosition;
        CinematicCamera = playerContainer.CinematicCamera;
        PlayerController = playerContainer.PlayerController;
        WeaponContainer = playerContainer.WeaponContainer;
        UIBluer = playerContainer.UIBluer;
    }
}