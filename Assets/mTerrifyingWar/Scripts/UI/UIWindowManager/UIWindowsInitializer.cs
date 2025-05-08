using System;
using UnityEngine;
using Zenject;

public class UIWindowsInitializer : MonoBehaviour
{
    [Inject] private readonly UIWindowService _windowService;
    
    [SerializeField] private UIWindowEntry[] _windows;

    private void OnEnable()
    {
        SubscribeWindows();
    }

    private void OnDisable()
    {
        UnsubscribeWindows();
    }

    private void SubscribeWindows()
    {
        foreach (var entry in _windows)
        {
            _windowService.SubscribeWindow(entry.type, entry.Window);
        }
    }
    
    private void UnsubscribeWindows()
    {
        foreach (var entry in _windows)
        {
            _windowService.UnsubscribeWindow(entry.type);
        }
    }
}

[Serializable]
public class UIWindowEntry
{
    public WindowType type;
    public UIWindow Window;
}