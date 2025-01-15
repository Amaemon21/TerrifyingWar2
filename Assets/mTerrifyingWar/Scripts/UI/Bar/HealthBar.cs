using Zenject;

public class HealthBar : Bar
{
    [Inject] private PlayerProvider _playerProvider;

    protected override void OnEnable()
    {
        _playerProvider.PlayerHealth.HealthChanged += OnBarChanged;
    }

    protected override void OnDisable()
    {
        _playerProvider.PlayerHealth.HealthChanged -= OnBarChanged;
    }
}