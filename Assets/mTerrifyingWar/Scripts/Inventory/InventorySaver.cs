using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class InventorySaver : MonoBehaviour
{
    [Inject] private readonly IStorageService _storageService;
    [Inject] private readonly DisplayProvider _displayProvider;
    
    [SerializeField] private InventoryDatabase _inventoryDatabase;
    [SerializeField] private Inventory _inventory;
    
    private SaveData _saveData;
    private InventoryEntity inventoryEntity;
    
    private void Awake()
    {
        _storageService.Load(LoadData);
    }

    private void Start()
    {
        _inventory = _displayProvider.Inventory;
        _inventoryDatabase = _inventory.InventoryDatabase;
    }

    public void AddItem(InventoryItemConfig inventoryItemConfig)
    {
        if (inventoryItemConfig.IsStackable)
        {
            ItemEntity foundItem = _saveData.InventoryEntity.FindItemByID(inventoryItemConfig.ItemID);
            
            if (foundItem != null)
            {
                foundItem.Count += inventoryItemConfig.ItemCount;
            }
            else
            {
                ItemEntity itemEntity = new ItemEntity
                {
                    ItemId = inventoryItemConfig.ItemID,
                    Count = inventoryItemConfig.ItemCount,
                };
                
                inventoryEntity.Items.Add(itemEntity);
            }
        }
        else
        {
            if (inventoryItemConfig is WeaponInventoryItemConfig weaponInventoryItemConfig)
            {
                var weaponItemEntity = new WeaponItemEntity()
                {
                    ItemId = inventoryItemConfig.ItemID,
                    Count = inventoryItemConfig.ItemCount,
                    CurrentAmmo = weaponInventoryItemConfig.CurrentAmmo,
                    IsEquipped = true,
                };
                
                inventoryEntity.Items.Add(weaponItemEntity);
            }
            else
            {
                ItemEntity itemEntity = new ItemEntity
                {
                    ItemId = inventoryItemConfig.ItemID,
                    Count = inventoryItemConfig.ItemCount,
                };
                
                inventoryEntity.Items.Add(itemEntity);
            }
        }
        
        _storageService.Save(_saveData);
    }
    
    public void RemoveItem(InventoryItemConfig inventoryItemConfig, int amount = 1)
    {
        var itemsToRemove = new List<ItemEntity>();

        foreach (var item in inventoryEntity.Items)
        {
            if (item.ItemId == inventoryItemConfig.ItemID)
            {
                if (inventoryItemConfig.IsStackable)
                {
                    item.Count -= amount;

                    if (item.Count <= 0)
                    {
                        itemsToRemove.Add(item);
                    }
                }
                else
                {
                    itemsToRemove.Add(item);
                }
            }
        }
        
        foreach (var item in itemsToRemove)
        {
            inventoryEntity.Items.Remove(item);
        }

        _storageService.Save(_saveData);
    }
    
    private void LoadData(SaveData saveData)
    {
        _saveData = saveData;
        inventoryEntity = _saveData.InventoryEntity;

        LoadInventory();
    }
    
    private void LoadInventory()
    {
        if (inventoryEntity.Items != null && inventoryEntity.Items.Any())
        {
            foreach (var itemData in inventoryEntity.Items)
            {
                InventoryItemConfig item = _inventoryDatabase.FindItemByID(itemData.ItemId);
                
                if (item is WeaponInventoryItemConfig weaponInventoryItemConfig)
                {
                    if (itemData is WeaponItemEntity weaponInventoryItemData)
                    {
                        weaponInventoryItemConfig.SetCurrentAmmo(weaponInventoryItemData.CurrentAmmo);
                        _inventory.AddItem(item, true);
                    }
                }
                else
                {
                    item.ResetCount();
                    item.AddCount(itemData.Count);
                    _inventory.AddItem(item, true);
                }
            }
        }
    }
}