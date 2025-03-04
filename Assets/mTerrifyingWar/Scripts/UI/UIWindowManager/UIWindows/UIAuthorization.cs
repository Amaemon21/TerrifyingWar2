using DG.Tweening;
using UnityEngine;

public class UIAuthorization: UIWindow
{
    [SerializeField] private CanvasGroup _canvasGroup;

    protected override void OnOpen()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1f, 1f);
    }
}