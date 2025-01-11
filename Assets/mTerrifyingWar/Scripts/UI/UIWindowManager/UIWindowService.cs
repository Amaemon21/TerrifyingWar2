using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIWindowService
{
    [Inject] private CursorStateService _cursorStateService;
    
    private readonly Dictionary<WindowType, UIWindow> _windows = new();

    public void SubscribeWindow(WindowType windowType, UIWindow window)
    {
        if (!_windows.ContainsKey(windowType))
        {
            _windows.Add(windowType, window);
        }
    }
    
    public void UnsubscribeWindow(WindowType windowType)
    {
        if (_windows.TryGetValue(windowType, out UIWindow window))
        {
            if (window.IsOpen)
            {
                window.Close(); 
            }
            
            _windows.Remove(windowType);
        }
        else
        {
            Debug.Log($"Window with type {windowType} not found");
        }
    }

    public void ToogleWindow(WindowType windowType)
    {
        if (_windows.TryGetValue(windowType, out UIWindow windowToOpen))
        {
            if (windowToOpen.IsOpen)
            {
                _cursorStateService.DisableCursor();
                windowToOpen.Close();
            }
            else
            {
                CloseAllWindows();
                
                _cursorStateService.EnableCursor();
                windowToOpen.Open();
            }
        }   
    }
    
    public UIWindow OpenWindow(WindowType windowType)
    {
        foreach (var window in _windows.Values)
        {
            if (window.IsOpen)
            {
                _cursorStateService.DisableCursor();
                window.Close();
            }
        }
        
        if (_windows.TryGetValue(windowType, out UIWindow windowToOpen))
        {
            _cursorStateService.EnableCursor();
            windowToOpen.Open();
            return windowToOpen;
        }      
        
        Debug.Log($"Window with type {windowType} not found");
        return null;
    }
    
    public void CloseWindow(WindowType windowType)
    {
        if (_windows.TryGetValue(windowType, out UIWindow window))
        {
            if (window.IsOpen)
            {
                window.Close();
            }        
            else
            {
                Debug.Log($"Window with type {windowType} is already closed.");
            }
        }
        else
        {
            Debug.Log($"Window with type {windowType} not found");
        }
    }

    public void CloseAllWindows()
    {
        foreach (var window in _windows.Values)
        {
            if (window.IsOpen)
            {
                window.Close();
            }
        }
    }
    
    public void CloseAndRemoveAllWindows()
    {
        foreach (var window in _windows.Values)
        {
            if (window.IsOpen)
            {
                window.Close();
            }
        }
        
        _windows.Clear();
    }
    
    public bool HasAnyWindowOpen()
    {
        foreach (var window in _windows.Values)
        {
            if (window.IsOpen)
            {
                return true;
            }
        }
        return false;
    }
}
