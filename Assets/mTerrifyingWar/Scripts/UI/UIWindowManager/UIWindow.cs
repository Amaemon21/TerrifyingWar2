using UnityEngine;

public abstract class UIWindow : MonoBehaviour
{    
    public bool IsOpen { get; private set; } = false;

    public void Open()
    {
        gameObject.SetActive(true);
        IsOpen = true;
        OnOpen();
    }
    
    public void Close()
    {
        gameObject.SetActive(false);
        IsOpen = false;
        OnClose();
    }

    protected virtual void OnOpen() { }
    protected virtual void OnClose() { }
}