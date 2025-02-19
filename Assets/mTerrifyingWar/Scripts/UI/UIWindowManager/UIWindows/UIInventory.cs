using UnityEngine;
using UnityEngine.UI;

public class UIInventory : UIWindow
{
    [SerializeField] private GameObject _menu;
    
    [SerializeField] private Image _line;
    [SerializeField] private Sprite _sprite;
    
    protected override void OnOpen()
    {
        _line.sprite = _sprite;
        
        _menu.SetActive(IsOpen);
    }

    protected override void OnClose()
    {
        _menu.SetActive(IsOpen);
    }
}