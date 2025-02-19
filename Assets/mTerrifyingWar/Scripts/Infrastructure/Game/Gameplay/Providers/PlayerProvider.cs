using UnityEngine;

public class PlayerProvider
{
    public PlayerController PlayerController { get; private set; }
    public WeaponRecoilAndShake WeaponRecoilAndShake { get; private set; }
    public Camera MainCamera { get; private set; }
    public Camera WeaponCamera { get; private set; }

    public void Setup(PlayerContainer playerContainer)
    {
        PlayerController = playerContainer.PlayerController;
        WeaponRecoilAndShake = playerContainer.WeaponRecoilAndShake;
        MainCamera = playerContainer.MainCamera;
        WeaponCamera = playerContainer.WeaponCamera;
    }

    public void EnablePlaeyr()
    {
        PlayerController.enabled = true;
    }

    public void DisablePlaeyr()
    {
        PlayerController.enabled = false;
    }
}