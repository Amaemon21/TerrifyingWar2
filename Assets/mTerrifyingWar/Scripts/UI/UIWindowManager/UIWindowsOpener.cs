using UnityEngine;
using Zenject;

public class UIWindowsOpener : MonoBehaviour
{
    [Inject] private readonly UIWindowService _uiWindowService;
    
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

        if (Input.GetKeyDown(KeyCode.I))
        {
            _uiWindowService.ToogleWindow(WindowType.Inventory);
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