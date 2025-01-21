using DG.Tweening;
using MVVM;
using R3;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HealthView : View
{
    [Inject] private readonly PlayerHealthViewModel _playerHealthViewModel;
    
    [SerializeField] private Image _fillImage;

    private void OnEnable()
    { 
        Disposable = _playerHealthViewModel.Health.Subscribe(OnHealthChanged);
    }

    private void OnHealthChanged(float currentHealth)
    {
        _fillImage.DOFillAmount(currentHealth, 0.5f);
    }
}