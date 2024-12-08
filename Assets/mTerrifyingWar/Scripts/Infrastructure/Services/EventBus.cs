using UnityEngine.Events;

public class EventBus
{
    public UnityEvent DisplayItemsChanged { get; } = new();

    public void HandleDisplayItemsChanged() => DisplayItemsChanged?.Invoke();
    
    public UnityEvent<int, int, Weapon> DisplayAmmoChanged { get; } = new();

    public void HandleDisplayAmmoChanged(int currentAmmo, int availableAmmo, Weapon weapon) => DisplayAmmoChanged?.Invoke(currentAmmo, availableAmmo, weapon);
    
    public UnityEvent ItemAddedInventoryChanged { get; } = new();

    public void HandleItemAddedInventoryChanged() => ItemAddedInventoryChanged?.Invoke();
    
    public UnityEvent<InventoryItemConfig> RemoveItemToInventoryChanged { get; } = new();

    public void HandleRemoveItemToInventoryChanged(InventoryItemConfig inventoryItemConfig) => RemoveItemToInventoryChanged?.Invoke(inventoryItemConfig);
}