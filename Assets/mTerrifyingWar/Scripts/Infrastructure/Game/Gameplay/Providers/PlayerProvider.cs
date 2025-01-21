public class PlayerProvider
{
    public PlayerController PlayerController { get; private set; }
    public PlayerStamina PlayerStamina { get; private set; }

    public void Setup(PlayerController playerController)
    {
        PlayerController = playerController;
        PlayerStamina = playerController.GetComponent<PlayerStamina>();
    }
}