using Zenject;

public class StaminaBar : Bar
{
    [Inject] private PlayerProvider _playerProvider;

    protected override void OnEnable()
    {
        _playerProvider.PlayerStamina.StaminaChanged += OnBarChanged;
    }

    protected override void OnDisable()
    {
        _playerProvider.PlayerStamina.StaminaChanged -= OnBarChanged;
    }
}