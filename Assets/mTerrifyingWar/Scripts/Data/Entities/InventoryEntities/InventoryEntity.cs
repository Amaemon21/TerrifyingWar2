using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class InventoryEntity
{
    public List<ItemEntity> Items { get; set; } = new();
    
    public ItemEntity FindItemByID(string id)
    {
        if (string.IsNullOrEmpty(id))
            return null;

        ItemEntity item = Items.FirstOrDefault(config => config.ItemId == id);

        if (item != null)
            return item;
        
        return null;
    }
}