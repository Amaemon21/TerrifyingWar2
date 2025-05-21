using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class InventorySaver : MonoBehaviour, ISavedProgressReader
{
    [Inject] private readonly IStorageService _storageService;
    [Inject] private readonly DisplayProvider _displayProvider;
    
    private GameState _gameState;
    
    public void LoadProgress(GameState gameState)
    {
        _gameState = gameState;
        
        LoadInventory();
    }

    public void AddItem(InventoryItemConfig inventoryItemConfig, bool isEquipped = false)
    {
        if (inventoryItemConfig.IsStackable)
        {
            ItemEntity foundItem = _gameState.Entities.OfType<ItemEntity>().FirstOrDefault(e => e.ItemId == inventoryItemConfig.ItemID);
            
            if (foundItem != null)
            {
                foundItem.Count += inventoryItemConfig.ItemCount;
            }
            else
            {
                _gameState.Entities.Add(new ItemEntity
                {
                    EntityId = _gameState.GetNewId(),
                    ItemId = inventoryItemConfig.ItemID,
                    Count = inventoryItemConfig.ItemCount,
                });
            }
        }
        else
        {
            _gameState.Entities.Add(new ItemEntity
            {
                EntityId = _gameState.GetNewId(),
                ItemId = inventoryItemConfig.ItemID,
                Count = inventoryItemConfig.ItemCount,
            });
        }
    }

    public void WeaponAddItem(WeaponInventoryItemConfig weaponInventoryItemConfig, bool isEquipped = false)
    {
        if (!weaponInventoryItemConfig.IsStackable)
        {
            int id = _gameState.GetNewId();
            weaponInventoryItemConfig.SetId(id);

            WeaponItemEntity weaponItemEntity = new WeaponItemEntity
            {
                EntityId = id,
                ItemId = weaponInventoryItemConfig.ItemID,
                Count = weaponInventoryItemConfig.ItemCount,
                IsEquipped = isEquipped,
                CurrentAmmo = weaponInventoryItemConfig.CurrentAmmo,
            };

            weaponInventoryItemConfig.SetEntity(weaponItemEntity);
                
            _gameState.Entities.Add(weaponItemEntity);
        }
    }
    
    public void RemoveItem(InventoryItemConfig inventoryItemConfig, int amount = 1)
    {
        List<ItemEntity> itemsToRemove = _gameState.Entities
            .OfType<ItemEntity>()
            .Where(item => item.ItemId == inventoryItemConfig.ItemID)
            .Where(item => !inventoryItemConfig.IsStackable || (item.Count -= amount) <= 0)
            .ToList();
        
        foreach (var item in itemsToRemove)
        {
            _gameState.Entities.Remove(item);
        }
    }
    
    private void LoadInventory()
    {
        foreach (ItemEntity itemEntity in _gameState.Entities.OfType<ItemEntity>())
        {
            InventoryItemConfig item = _displayProvider.Inventory.InventoryComponent.InventoryDatabase.FindItemByID(itemEntity.ItemId);
            InventoryItemConfig copyItem = Instantiate(item);
            
            if (itemEntity is WeaponItemEntity weaponItemEntity && copyItem is WeaponInventoryItemConfig weaponInventoryItemConfig)
            {
                weaponInventoryItemConfig.SetId(weaponItemEntity.EntityId);
                weaponInventoryItemConfig.SetEntity(weaponItemEntity);
                weaponInventoryItemConfig.SetCurrentAmmo(weaponItemEntity.CurrentAmmo);

                if (weaponItemEntity.IsEquipped)
                {
                    _displayProvider.Inventory.AddEquipableItem(weaponInventoryItemConfig, weaponItemEntity.SlotType, weaponItemEntity.IsEquipped);
                }
                else
                {
                    _displayProvider.Inventory.AddItem(weaponInventoryItemConfig, true);
                }
            }
            else
            {
                copyItem.ResetCount();
                copyItem.AddCount(itemEntity.Count);
                _displayProvider.Inventory.AddItem(copyItem, true);
            }
        }
    }
}
