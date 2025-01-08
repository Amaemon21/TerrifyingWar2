using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryData
{
    public List<ItemData> ItemDatas = new List<ItemData>();
    public List<WeaponItemData> WeaponItemDatas = new List<WeaponItemData>();

    public void RemoveItemById(string id)
    {
        if (ItemDatas.Count != 0)
        {
            for (int i = ItemDatas.Count - 1; i >= 0; i--)
            {
                if (ItemDatas[i].ItemId == id)
                {
                    ItemDatas.RemoveAt(i);
                }
            }
        }
    }
    
    public ItemData FindItemByID(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogWarning("FindItemByID: id is null or empty.");
            return null;
        }

        foreach (var item in ItemDatas)
        {
            Debug.Log($"Checking item: {item.ItemId}");
            if (item.ItemId == id)
            {
                Debug.Log($"Item found: {item.ItemId}");
                return item;
            }
        }

        Debug.LogWarning($"Item with ID '{id}' not found.");
        return null;
    }
    
    public WeaponItemData FindWeaponItemByID(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogWarning("FindItemByID: id is null or empty.");
            return null;
        }

        foreach (var item in WeaponItemDatas)
        {
            if (item.ItemId == id)
            {
                if (item is WeaponItemData weaponItemData)
                {
                    Debug.Log($"Item found: {item.ItemId}");
                    return weaponItemData;
                }
            }
        }

        Debug.LogWarning($"Item with ID '{id}' not found.");
        return null;
    }

}

[Serializable]
public class ItemData
{
    public string ItemId;
    public int ItemCount;

    public ItemData(string itemId, int itemCount)
    {
        ItemId = itemId;
        ItemCount = itemCount;
    }
}

[Serializable]
public class WeaponItemData
{
    public string ItemId;
    public int ItemCount;
    public int AmmoCount;

    public WeaponItemData(string itemId, int itemCount, int ammoCount)
    {
        ItemId = itemId;
        ItemCount = itemCount;
        AmmoCount = ammoCount;
    }
}