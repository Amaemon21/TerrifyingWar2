using MVVM;
using R3;

public class PlayerHealthViewModel : ViewModel
{
    private readonly PlayerHealth _playerHealth;
    private readonly ReactiveProperty<float> _health = new();
    private readonly Subject<Unit> _dead = new();
    public Observable<float> Health => _health;
    public Observable<Unit> Dead => _dead;
    
    public PlayerHealthViewModel(PlayerHealth playerHealth)
    {
        _playerHealth = playerHealth;
    }
    
    public override void Initialize()
    { 
        Disposable = _playerHealth.Health.Subscribe(OnHealthChanged);
    }
    
    private void OnHealthChanged(float health)
    {
        _health.Value = health / _playerHealth.MaxHealth;

        if (health == 0)
            _dead.OnNext(Unit.Default);
    }
}