using Zenject;
using UnityEngine;
using DG.Tweening;

public class UIPause : UIWindow
{
    [Inject] private readonly IInputService _inputService;
    [Inject] private CursorStateService _cursorStateService;
    
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _panel;
    
    [Space(10)]
    [SerializeField] private float _hideX;
    [SerializeField] private float _showX;
    [SerializeField] private float _duration = 0.3f;
    
    protected override void OnOpen()
    {
        _inputService.DisablePlayerMap();
        _cursorStateService.EnableCursor();
        
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = true;
        
        _canvasGroup.DOFade(1, _duration).SetLink(_canvasGroup.gameObject);
        _panel.DOAnchorPosX(_showX, _duration).SetEase(Ease.OutBack).SetLink(_panel.gameObject);
    }

    protected override void OnClose()
    {
        _inputService.EnablePlayerMap();
        _cursorStateService.DisableCursor();
        
        _canvasGroup.DOFade(0, _duration).SetLink(_canvasGroup.gameObject);
        _panel.DOAnchorPosX(_hideX, _duration).SetEase(Ease.InBack).OnComplete(() => _canvasGroup.blocksRaycasts = false).SetLink(_panel.gameObject);
    }
}