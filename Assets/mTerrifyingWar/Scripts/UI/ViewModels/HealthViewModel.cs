using MVVM;
using R3;

public class HealthViewModel : ViewModel
{
    private readonly PlayerHealth _playerHealth;
    
    private readonly ReactiveProperty<float> _health = new();
    private readonly Subject<Unit> _plaeyrDeadSubject = new();
    
    public Observable<float> Health => _health;
    public Observable<Unit> PlaeyrDeadSubject => _plaeyrDeadSubject;
    
    public HealthViewModel(PlayerHealth playerHealth)
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
            _plaeyrDeadSubject.OnNext(Unit.Default);
    }
}