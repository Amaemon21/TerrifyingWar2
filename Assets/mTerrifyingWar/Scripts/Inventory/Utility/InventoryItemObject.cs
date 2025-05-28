using NaughtyAttributes;
using UnityEngine;

//[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(Rigidbody))]
public class InventoryItemObject : MonoBehaviour
{
    [SerializeField, Expandable] private InventoryItemConfig _inventoryItemConfig;
    
    //public Outline Outline { get; private set; }
    public InventoryItemConfig InventoryItemConfig { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    
    public void SetConfig(InventoryItemConfig inventoryItemConfig)
    {
        InventoryItemConfig = inventoryItemConfig;
    }
    
    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        //Outline = GetComponent<Outline>();
        //Outline.enabled = false;
        
        InitializeItem();
    }

    private void InitializeItem()
    {
        InventoryItemConfig = Instantiate(_inventoryItemConfig);

        if (InventoryItemConfig is WeaponInventoryItemConfig weaponInventoryItemConfig)
        {
            if (weaponInventoryItemConfig.ScopeInventoryItemConfig != null)
            {
                var scope = Instantiate(weaponInventoryItemConfig.ScopeInventoryItemConfig.Scope, transform);
                scope.transform.localPosition = weaponInventoryItemConfig.ScopeInventoryItemConfig.Position;
                scope.transform.localRotation = Quaternion.identity; 
            }
        }
    }
}