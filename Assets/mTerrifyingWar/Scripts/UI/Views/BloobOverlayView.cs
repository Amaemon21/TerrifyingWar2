using System.Collections;
using DG.Tweening;
using MVVM;
using R3;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BloobOverlayView : View
{
    [Inject] private HealthViewModel _healthViewModel;
    
    [SerializeField] private Image _bloobOverlay;
    [SerializeField] private float _fadeDuration = 0.5f;
    [SerializeField] private float _visibleDuration = 0.3f;

    private Coroutine _overlayRoutine;
    
    private void OnEnable()
    {
        _healthViewModel.Health.Skip(2).Subscribe(UpdateBloobOverlay).AddTo(CompositeDisposable);
    }

    private void UpdateBloobOverlay(float currentHealth)
    {
        if (_overlayRoutine != null)
        {
            StopCoroutine(_overlayRoutine);
        }

        _overlayRoutine = StartCoroutine(ShowBloobOverlay(1 - currentHealth));
    }

    private IEnumerator ShowBloobOverlay(float targetAlpha)
    {
        _bloobOverlay.DOFade(targetAlpha, _fadeDuration);
        yield return new WaitForSeconds(_fadeDuration + _visibleDuration);
        
        _bloobOverlay.DOFade(0f, _fadeDuration);
    }
}