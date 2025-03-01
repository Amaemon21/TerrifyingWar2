using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindowService
{
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
                windowToOpen.Close();
            }
            else
            {
                CloseAllWindows();
                
                windowToOpen.Open();
            }
        }   
    }
    
    public void OpenWindow(WindowType windowType)
    {
        foreach (var window in _windows.Values)
        {
            if (window.IsOpen)
            {
                window.Close();
            }
        }
        
        if (_windows.TryGetValue(windowType, out UIWindow windowToOpen))
        {
            windowToOpen.Open();
        }
        else
        {
            Debug.Log($"Window with type {windowType} not found");
        }
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
            
            Object.Destroy(window.gameObject);
        }
        
        _windows.Clear();
    }

    public bool IsWindowOpened(WindowType windowType)
    {
        if (_windows.TryGetValue(windowType, out UIWindow window))
        {
            return window.IsOpen;
        }

        return false;
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