using UnityEngine;
using Zenject;

public class UIWindowsOpener : MonoBehaviour
{
    [Inject] private readonly UIWindowService _uiWindowService;
    [Inject] private readonly IInputService _inputService;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_uiWindowService.HasAnyWindowOpen())
            {
                _uiWindowService.CloseAllWindows();
            }
            else
            {
                _uiWindowService.ToogleWindow(WindowType.Pause);
            }
        }

        if (_inputService.IsInventory)
        {
            if (_uiWindowService.IsWindowOpened(WindowType.Inventory))
            {
                _uiWindowService.CloseWindow(WindowType.Inventory);
            }
            else
            {
                _uiWindowService.OpenWindow(WindowType.Inventory);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.M))
        {
            _uiWindowService.ToogleWindow(WindowType.Map);
        }
    }
    
    public void ToogleWindowInventory()
    {
        _uiWindowService.OpenWindow(WindowType.Inventory);
    }

    public void ToogleWindowMap()
    {
        _uiWindowService.OpenWindow(WindowType.Map);
    }
}