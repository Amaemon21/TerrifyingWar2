using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(Rigidbody))]
public class InventoryItemObject : MonoBehaviour
{
    [SerializeField, Expandable] private InventoryItemConfig _inventoryItemConfig;
    
    public Outline Outline { get; private set; }
    public InventoryItemConfig InventoryItemConfig { get; private set; }
    
    public void SetConfig(InventoryItemConfig inventoryItemConfig)
    {
        InventoryItemConfig = inventoryItemConfig;
    }
    
    private void Awake()
    {
        InitializeItem();
        
        Outline = GetComponent<Outline>();
        Outline.enabled = false;
    }

    private void InitializeItem()
    {
        InventoryItemConfig = Instantiate(_inventoryItemConfig);
    }
}