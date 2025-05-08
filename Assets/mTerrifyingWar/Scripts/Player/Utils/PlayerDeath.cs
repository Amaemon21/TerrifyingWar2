using MVVM;
using R3;
using Zenject;

public class PlayerDeath : View
{
    [Inject] private readonly HealthViewModel _healthViewModel;
    [Inject] private readonly UIWindowService _windowService;
    
    private bool _isDead = false;

    private void OnEnable()
    {
        _healthViewModel.PlaeyrDeadSubject.Subscribe(Die).AddTo(CompositeDisposable);
    }

    private void Die(Unit unit)
    {
        if (_isDead) 
            return;

        _isDead = true;

        _windowService.OpenWindow(WindowType.GameEnd);
    }
}