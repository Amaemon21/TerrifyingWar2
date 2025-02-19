using UnityEngine;
using Zenject;

public class UIWindowsOpener : MonoBehaviour
{
    [Inject] private readonly PlayerProvider _playerProvider;
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
                _playerProvider.EnablePlaeyr();
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

    private void ToogleWindow(WindowType type)
    {
        if (_uiWindowService.IsWindowOpened(type))
        {
            _cursorStateService.DisableCursor();
            _playerProvider.EnablePlaeyr();
            _uiWindowService.CloseWindow(type);
        }
        else
        {
            _cursorStateService.EnableCursor();
            _playerProvider.DisablePlaeyr();
            _uiWindowService.OpenWindow(type);
        }
    }
}