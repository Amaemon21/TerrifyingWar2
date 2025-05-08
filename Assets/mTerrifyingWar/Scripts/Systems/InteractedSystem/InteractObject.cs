using NaughtyAttributes;
using UnityEngine;

public abstract class InteractObject : MonoBehaviour
{
    [field: SerializeField, Expandable] public InteractObjectConfig InteractObjectConfig { get; private set; }

    public void Interact()
    {
        OnInteract();
    }

    protected virtual void OnInteract() {}
}