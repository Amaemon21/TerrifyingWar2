public class PlayerProvider
{
    public PlayerController PlayerController { get; private set; }
    public PlayerHealth PlayerHealth { get; private set; }
    public PlayerStamina PlayerStamina { get; private set; }

    public void Setup(PlayerController playerController)
    {
        PlayerController = playerController;
        PlayerHealth = playerController.GetComponent<PlayerHealth>();
        PlayerStamina = playerController.GetComponent<PlayerStamina>();
    }
}