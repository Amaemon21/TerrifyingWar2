using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UICraft : UIWindow
{
    [Inject] private readonly IInputService _inputService;
    
    [SerializeField] private GameObject _menu;
    
    [SerializeField] private Image _line;
    [SerializeField] private Sprite _sprite;
    
    protected override void OnOpen()
    {
        _line.sprite = _sprite;

        _menu.SetActive(IsOpen);
        
        _inputService.DisablePlayerMap();
    }

    protected override void OnClose()
    {
        _menu.SetActive(IsOpen);
        
        _inputService.EnablePlayerMap();
    }
}