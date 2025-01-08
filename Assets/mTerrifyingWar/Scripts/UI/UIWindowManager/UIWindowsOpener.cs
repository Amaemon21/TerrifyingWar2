using UnityEngine;
using Zenject;

public class UIWindowsOpener : MonoBehaviour
{
    [Inject] private readonly UIManager _uiManager;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_uiManager.HasAnyWindowOpen())
            {
                _uiManager.CloseAllWindows();
            }
            else
            {
                _uiManager.ToogleWindow(WindowType.Pause);
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            _uiManager.ToogleWindow(WindowType.Inventory);
        }
        
        if (Input.GetKeyDown(KeyCode.M))
        {
            _uiManager.ToogleWindow(WindowType.Map);
        }
    }
    
    public void ToogleWindowInventory()
    {
        _uiManager.OpenWindow(WindowType.Inventory);
    }

    public void ToogleWindowMap()
    {
        _uiManager.OpenWindow(WindowType.Map);
    }
}