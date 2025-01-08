using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIInventory : UIWindow
{
    [Inject] private CursorStateService _cursorStateService;
    [Inject] private IInputService _inputService;
    
    [SerializeField] private GameObject _menu;
    
    [SerializeField] private Image _line;
    [SerializeField] private Sprite _sprite;
    
    protected override void OnOpen()
    {
        _line.sprite = _sprite;
        
        _menu.SetActive(IsOpen);
        
        _cursorStateService.EnableCursor();
        _inputService.Disable();
    }

    protected override void OnClose()
    {
        _menu.SetActive(IsOpen);
        
        _cursorStateService.DisableCursor();
        _inputService.Enable();
    }
}