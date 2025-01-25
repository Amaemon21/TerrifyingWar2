using UnityEngine;

public abstract class UIWindow : MonoBehaviour
{    
    public bool IsOpen { get; private set; } = false;

    public virtual void Open()
    {
        gameObject.SetActive(true);
        IsOpen = true;
        OnOpen();
    }
    
    public virtual void Close()
    {
        gameObject.SetActive(false);
        IsOpen = false;
        OnClose();
    }

    protected virtual void OnOpen() { }
    protected virtual void OnClose() { }
}