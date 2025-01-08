//using EPOOutline;
using NaughtyAttributes;
using UnityEngine;

//[RequireComponent(typeof(Outlinable))]
[RequireComponent(typeof(Rigidbody))]
public class InventoryItemObject : MonoBehaviour
{
    [SerializeField, Expandable] private InventoryItemConfig _inventoryItemConfig;
    
    //[field: SerializeField, Space(10)] public Outlinable Outline { get; private set; }
    
    public InventoryItemConfig InventoryItemConfig { get; private set; }

    private void OnValidate()
    {
        //Outline ??= GetComponent<Outlinable>();
    }

    public void SetConfig(InventoryItemConfig inventoryItemConfig)
    {
        InventoryItemConfig = inventoryItemConfig;
    }
    
    private void Awake()
    {
        InitializeItem();
        
        //Outline.enabled = false;
    }

    private void InitializeItem()
    {
        InventoryItemConfig = Instantiate(_inventoryItemConfig);
    }
}