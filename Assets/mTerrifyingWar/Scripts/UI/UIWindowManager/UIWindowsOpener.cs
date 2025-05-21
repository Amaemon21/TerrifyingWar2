using UnityEngine;
using Zenject;

public class UIWindowsOpener : MonoBehaviour
{
    [Inject] private readonly CursorStateService _cursorStateService;
    [Inject] private readonly UIWindowService _uiWindowService;
    [Inject] private readonly IInputService _inputService;
    
    private void Update()
    {
        if (_inputService.IsEscape)
        {
            if (_uiWindowService.HasAnyWindowOpen())
            {
                _cursorStateService.DisableCursor();
                _uiWindowService.CloseAllWindows();
            }
            else
            {
                ToogleWindow(WindowType.Pause);
            }
        }

        if (_inputService.IsInventory)
            ToogleWindow(WindowType.Inventory);
        
        if (_inputService.IsMap)
            ToogleWindow(WindowType.Map);        
    }
    
    public void ToogleWindowInventory()
    {
        _uiWindowService.OpenWindow(WindowType.Inventory);
    }

    public void ToogleWindowMap()
    {
        _uiWindowService.OpenWindow(WindowType.Map);
    }
    
    public void ToogleWindowTask()
    {
        _uiWindowService.OpenWindow(WindowType.Task);
    }
    
    public void ToogleWindowCraft()
    {
        _uiWindowService.OpenWindow(WindowType.Craft);
    }

    public void ClosePauseMenu()
    {
        _uiWindowService.CloseWindow(WindowType.Pause);
    }

    private void ToogleWindow(WindowType type)
    {
        if (_uiWindowService.IsWindowOpened(type))
        {
            _cursorStateService.DisableCursor();
            _uiWindowService.CloseWindow(type);
        }
        else
        {
            _cursorStateService.EnableCursor();
            _uiWindowService.OpenWindow(type);
        }
    }
}