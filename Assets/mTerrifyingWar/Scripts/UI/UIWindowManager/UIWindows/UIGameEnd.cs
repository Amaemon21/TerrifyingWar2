using Zenject;

public class UIGameEnd : UIWindow
{
    [Inject] private CursorStateService _cursorStateService;
    
    protected override void OnOpen()
    {
        _cursorStateService.EnableCursor();
    }

    protected override void OnClose()
    {
        _cursorStateService.DisableCursor();
    }
}