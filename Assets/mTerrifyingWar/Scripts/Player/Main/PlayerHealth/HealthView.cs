using DG.Tweening;
using MVVM;
using R3;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HealthView : View
{
    [Inject] private readonly HealthViewModel _healthViewModel;
    
    [SerializeField] private Image _fillImage;

    private void OnEnable()
    {
        _healthViewModel.Health.Subscribe(OnHealthChanged).AddTo(CompositeDisposable);
    }

    private void OnHealthChanged(float value)
    {
        _fillImage.DOFillAmount(value, 0.5f);
    }
}