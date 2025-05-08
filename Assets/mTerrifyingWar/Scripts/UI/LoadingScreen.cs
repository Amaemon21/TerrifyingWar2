using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class LoadingScreen : MonoBehaviour
{
    private readonly float _fadeDuration = 0.5f;
    
    [SerializeField] private CanvasGroup _canvasGroup;
    
    private void Awake()
    {
        gameObject.SetActive(false);
        _canvasGroup.alpha = 0;
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
        _canvasGroup.alpha = 1;
    }

    public void Hide()
    {
        _canvasGroup.DOFade(0, _fadeDuration).OnComplete(() =>
        {
            gameObject.SetActive(false);
        }).SetLink(_canvasGroup.gameObject);
    }
}