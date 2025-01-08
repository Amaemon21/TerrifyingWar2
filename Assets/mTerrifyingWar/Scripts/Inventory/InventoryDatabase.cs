using System.Linq;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryDatabase", menuName = "Inventory/InventoryDatabase")]
public class InventoryDatabase : ScriptableObject
{
    [field: SerializeField, Expandable] public InventoryItemConfig[] InventoryItemConfigs;

    public InventoryItemConfig FindItemByID(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogWarning("Invalid ID: ID cannot be null or empty.");
            return null;
        }

        InventoryItemConfig item = InventoryItemConfigs.FirstOrDefault(config => config.ItemID == id);

        if (item != null)
        {
            Debug.Log($"Item found: {item.name}");
            return item;
        }
        
        Debug.LogWarning($"Item with ID '{id}' not found.");
        return null;
    }
}
