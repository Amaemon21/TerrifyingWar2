using System.Collections.Generic;
using UnityEngine;

public class LootGenerator
{
    private readonly InventoryDatabase _inventoryDatabase;
    private readonly int _minItems;
    private readonly int _maxItems;

    public LootGenerator(int minItems = 1, int maxItems = 1)
    {
        _inventoryDatabase = Resources.Load<InventoryDatabase>(AssetsPath.InventoryDatabasePath);
        
        _minItems = minItems;
        _maxItems = maxItems;
    }
    
    public List<InventoryItemConfig> GenerateLoot()
    {
        List<InventoryItemConfig> loots = new();

        int itemCount = Random.Range(_minItems, _maxItems + 1);

        for (int i = 0; i < itemCount; i++)
        {
            InventoryItemConfig item = GetWeightedRandomItem();
            if (item != null)
                loots.Add(item);
        }

        return loots;
    }
    
    private InventoryItemConfig GetWeightedRandomItem()
    {
        float totalWeight = 0f;
        foreach (InventoryItemConfig item in _inventoryDatabase.InventoryItemConfigs)
        {
            totalWeight += GetWeight(item.ItemRarity);
        }

        float randomValue = Random.value * totalWeight;
        float currentWeight = 0f;

        foreach (InventoryItemConfig item in _inventoryDatabase.InventoryItemConfigs)
        {
            currentWeight += GetWeight(item.ItemRarity);
            if (randomValue <= currentWeight)
                return item;
        }

        return null;
    }

    private float GetWeight(RarityType rarity)
    {
        return 1f / (int)rarity;
    }
}