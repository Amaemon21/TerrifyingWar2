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
        foreach (UIWindowEntry entry in _windows)
        {
            _windowService.SubscribeWindow(entry.Type, entry.Window);
        }
    }

    private void UnsubscribeWindows()
    {
        foreach (UIWindowEntry entry in _windows)
        {
            _windowService.UnsubscribeWindow(entry.Type);
        }
    }
}

[Serializable]
public class UIWindowEntry
{
    public WindowType Type;
    public UIWindow Window;
}