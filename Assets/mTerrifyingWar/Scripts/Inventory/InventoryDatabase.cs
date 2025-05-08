using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryDatabase", menuName = "Inventory/InventoryDatabase")]
public class InventoryDatabase : ScriptableObject
{
    [field: SerializeField, Expandable] public List<InventoryItemConfig> InventoryItemConfigs;

    public InventoryItemConfig FindItemByID(string id)
    {
        if (string.IsNullOrEmpty(id))
            return null;

        InventoryItemConfig item = InventoryItemConfigs.FirstOrDefault(config => config.ItemID == id);

        if (item != null)
            return item;
        
        return null;
    }

    [Button("Find Items")]
    public void FindItems()
    {
        InventoryItemConfig[] allItems = Resources.LoadAll<InventoryItemConfig>("Configs/Items");

        if (allItems.Length > 0)
        {
            Debug.Log($"Found {allItems.Length} items.");
        
            foreach (var item in allItems)
            {
                if (!InventoryItemConfigs.Contains(item))
                {
                    InventoryItemConfigs.Add(item);
                    Debug.Log($"Item added to the database: {item.name}");
                }
                else
                {
                    Debug.Log($"Item already exists in the database: {item.name}");
                }
            }
        }
        else
        {
            Debug.LogWarning("No items found in Resources/Configs/Items.");
        }
    }


    [Button("Generate Id")]
    public void GenerateId()
    {
        foreach (var item in InventoryItemConfigs)
        {
            if (string.IsNullOrEmpty(item.ItemID))
            {
                string id = $"{item.ItemName}_{Guid.NewGuid().ToString()}";
        
                item.SetupId(id);
            }
        }
    }
}
