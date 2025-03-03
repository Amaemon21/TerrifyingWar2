using System;
using UnityEngine;
using Zenject;

public class UIWindowsInitializer : MonoBehaviour
{
    [Inject] private readonly UIWindowService _windowService;
    
    [SerializeField] private UIWindowEntry[] _windows;

    private void Awake()
    {
        SubscribeWindows();
    }
    
    private void SubscribeWindows()
    {
        foreach (UIWindowEntry entry in _windows)
        {
            _windowService.SubscribeWindow(entry.Type, entry.Window);
        }
    }
}

[Serializable]
public class UIWindowEntry
{
    public WindowType Type;
    public UIWindow Window;
}