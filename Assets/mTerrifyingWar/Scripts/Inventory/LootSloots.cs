using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LootSloots : MonoBehaviour
{
    [Inject] private readonly DisplayProvider _displayProvider;
    
    private readonly List<InventoryItemCell> _inventoryItemsCells = new();   
    
    [SerializeField] private GameObject _lootSlotsMenu;
    
    private bool _isInitialized = false;

    public void Setup(List<InventoryItemConfig> inventoryItemConfigs)
    {
        _displayProvider.InventoryComponent.InventoryCellFactory.SpawnLootCells(_inventoryItemsCells);
        
        foreach (var item in inventoryItemConfigs)
        {
            AddItem(item);
        }
        
        _lootSlotsMenu.SetActive(true);
        
        _isInitialized = false;
    }

    private void OnDisable()
    {
        _lootSlotsMenu.SetActive(false);
    }

    public void AddItem(InventoryItemConfig config)
    {
        if (config.IsStackable)
            AddStackableItem(config);
        else
            AddUnstackableItem(config);
        
        DisplayItems();
    }
    
    public void DisplayItems()
    {
        foreach (var cell in _inventoryItemsCells)
        {
            cell.RedrawCell();
        }
    }

    private void AddStackableItem(InventoryItemConfig pickupedConfig)
    {
        foreach (InventoryItemCell item in _inventoryItemsCells)
        {
            InventoryItemConfig config = item.InventoryItemConfig;
 
            if (config != null)
            {
                if (config.ItemID == pickupedConfig.ItemID)
                {
                    config.AddCount(pickupedConfig.ItemCount);
                    
                    return;
                }
            }
        }

        AddUnstackableItem(pickupedConfig);
    }
    
    private void AddUnstackableItem(InventoryItemConfig pickupedConfig)
    {
        foreach (var cell in _inventoryItemsCells)
        {
            if (cell.InventoryItemConfig == null)
            {
                cell.SetItem(pickupedConfig);
                break;
            }
        }
    }
}