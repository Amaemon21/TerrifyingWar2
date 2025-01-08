using Zenject;

public class UIPause : UIWindow
{
    [Inject] private CursorStateService _cursorStateService;
    [Inject] private IInputService _inputService;
    
    protected override void OnOpen()
    {
        _cursorStateService.EnableCursor();
        _inputService.Disable();
    }

    protected override void OnClose()
    {
        _cursorStateService.DisableCursor();
        _inputService.Enable();
    }
}