using MVVM;
using R3;
using Zenject;

public class PlayerDeath : View
{
    [Inject] private readonly PlayerHealthViewModel _playerHealthViewModel;
    [Inject] private readonly UIWindowService _windowService;
    
    private bool _isDead = false;

    private void OnEnable()
    {
        Disposable = _playerHealthViewModel.Dead.Subscribe(Die);
    }

    private void Die(Unit unit)
    {
        if (_isDead) 
            return;

        _isDead = true;

        _windowService.OpenWindow(WindowType.GameEnd);
    }
}