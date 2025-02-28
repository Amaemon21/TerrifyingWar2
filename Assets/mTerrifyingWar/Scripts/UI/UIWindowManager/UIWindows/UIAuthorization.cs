using DG.Tweening;
using UnityEngine;

public class UIAuthorization: UIWindow
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _durationFade = 0.3f;
    
    protected override void OnOpen()
    {
        if (_canvasGroup.alpha < 1)
            _canvasGroup.DOFade(1, _durationFade);
    }

    protected override void OnClose()
    {
        if (_canvasGroup.alpha > 0)
            _canvasGroup.DOFade(0, _durationFade);
    }
}