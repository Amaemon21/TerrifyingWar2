using Zenject;
using UnityEngine;
using DG.Tweening;

public class UIPause : UIWindow
{
    [Inject] private readonly IInputService _inputService;
    [Inject] private CursorStateService _cursorStateService;
    
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _panel;
    [SerializeField] private float _animationDuration = 0.3f;
    [SerializeField] private Vector2 _openPositionOffset = new Vector2(200, 0);
    
    private Vector2 _originalPosition;
    
    private void Awake()
    {
        _originalPosition = _panel.anchoredPosition;
    }
    
    protected override void OnOpen()
    {
        _inputService.DisablePlayerMap();
        _cursorStateService.EnableCursor();
        
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = true;
        
        _canvasGroup.DOFade(1, _animationDuration);
        _panel.DOAnchorPos(_openPositionOffset, _animationDuration).SetEase(Ease.OutBack);
    }

    protected override void OnClose()
    {
        _inputService.EnablePlayerMap();
        _cursorStateService.DisableCursor();
        
        _canvasGroup.DOFade(0, _animationDuration);
        _panel.DOAnchorPos(_originalPosition, _animationDuration)
            .SetEase(Ease.InBack)
            .OnComplete(() => _canvasGroup.blocksRaycasts = false);
    }
}