using DG.Tweening;
using MVVM;
using R3;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BloobOverlay : View
{
    [Inject] private PlayerHealthViewModel _playerHealthModel;
    
    [SerializeField] private Image _bloobOverlay;
    [SerializeField] private float _fadeDuration = 0.5f;

    private void OnEnable()
    {
        Disposable = _playerHealthModel.Health.Subscribe(UpdateBloobOverlay);
    }

    private void UpdateBloobOverlay(float currentHealth)
    {
        _bloobOverlay.DOFade(1 - currentHealth, _fadeDuration);
    }
}