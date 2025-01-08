using Zenject;
using UnityEngine;
using UnityEngine.UI;

public class UIMap : UIWindow
{
    [Inject] private IInputService _inputService;

    [SerializeField] private GameObject _menu;
    
    [SerializeField] private Image _line;
    [SerializeField] private Sprite _sprite;
    
    protected override void OnOpen()
    {
        _line.sprite = _sprite;

        _menu.SetActive(IsOpen);
        
        _inputService.Disable();
    }

    protected override void OnClose()
    {
        _menu.SetActive(IsOpen);
        
        _inputService.Enable();
    }
}