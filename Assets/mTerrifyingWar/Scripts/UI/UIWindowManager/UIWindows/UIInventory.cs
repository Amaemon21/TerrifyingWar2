using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIInventory : UIWindow
{
    [Inject] private CursorStateService _cursorStateService;
    
    [SerializeField] private GameObject _menu;
    
    [SerializeField] private Image _line;
    [SerializeField] private Sprite _sprite;
    
    protected override void OnOpen()
    {
        _line.sprite = _sprite;
        
        _menu.SetActive(IsOpen);
        
        _cursorStateService.EnableCursor();
    }

    protected override void OnClose()
    {
        _menu.SetActive(IsOpen);
        
        _cursorStateService.DisableCursor();
    }
}