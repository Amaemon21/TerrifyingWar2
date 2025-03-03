using Zenject;

public class UIGameEnd : UIWindow
{
    [Inject] private readonly IInputService _inputService;
    [Inject] private CursorStateService _cursorStateService;
    
    protected override void OnOpen()
    {
        _inputService.DisablePlayerMap();
        _cursorStateService.EnableCursor();
    }

    protected override void OnClose()
    {
        _inputService.EnablePlayerMap();
        _cursorStateService.DisableCursor();
    }
}