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
    
    private GameState _gameState;
    
    private void Awake()
    {
        //_storageService.Load(LoadData);
    }

    private void Start()
    {
        _inventory = _displayProvider.Inventory;
        _inventoryDatabase = _inventory.InventoryDatabase;
    }

    public void AddItem(InventoryItemConfig inventoryItemConfig)
    {
        return;
        
        if (inventoryItemConfig.IsStackable)
        {
            ItemEntity foundItem = null;
            
            foreach (var item in _gameState.Entities)
            {
                if (item is ItemEntity itemEntity)
                {
                    if (itemEntity.ItemId == inventoryItemConfig.ItemID)
                    { 
                        foundItem = itemEntity;
                    }
                }
            }
            
            if (foundItem != null)
            {
                foundItem.Count += inventoryItemConfig.ItemCount;
            }
            else
            {
                ItemEntity itemEntity = new ItemEntity
                {
                    EntityId = _gameState.GetNewId(),
                    ItemId = inventoryItemConfig.ItemID,
                    Count = inventoryItemConfig.ItemCount,
                };
                
                _gameState.Entities.Add(itemEntity);
            }
        }
        else
        {
            if (inventoryItemConfig is WeaponInventoryItemConfig weaponInventoryItemConfig)
            {
                int id = _gameState.GetNewId();
                
                weaponInventoryItemConfig.SetId(id);
                
                var weaponItemEntity = new WeaponItemEntity()
                {
                    EntityId = id,
                    ItemId = inventoryItemConfig.ItemID,
                    Count = inventoryItemConfig.ItemCount,
                    IsEquipped = true,
                };
                
                weaponItemEntity.CurrentAmmo = weaponInventoryItemConfig.CurrentAmmo;
                
                _gameState.Entities.Add(weaponItemEntity);
            }
            else
            {
                ItemEntity itemEntity = new ItemEntity
                {
                    ItemId = inventoryItemConfig.ItemID,
                    Count = inventoryItemConfig.ItemCount,
                };
                
                _gameState.Entities.Add(itemEntity);
            }
        }
        
        //_storageService.Save(_gameState);
    }
    
    public void RemoveItem(InventoryItemConfig inventoryItemConfig, int amount = 1)
    {
        return;
        
        var itemsToRemove = new List<Entity>();

        foreach (var item in _gameState.Entities)
        {
            if (item is ItemEntity itemEntity)
            {
                if (itemEntity.ItemId == inventoryItemConfig.ItemID)
                {
                    if (inventoryItemConfig.IsStackable)
                    {
                        itemEntity.Count -= amount;

                        if (itemEntity.Count <= 0)
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
        }
        
        foreach (var item in itemsToRemove)
        {
            _gameState.Entities.Remove(item);
        }
        
        //_storageService.Save(_gameState);
    }
    
    private void LoadData(GameState gameState)
    {
        _gameState = gameState;

        LoadInventory();
    }
    
    private void LoadInventory()
    {
        if (_gameState.Entities.Any())
        {
            foreach (var itemData in _gameState.Entities)
            {
                if (itemData is ItemEntity itemEntity)
                {
                    InventoryItemConfig item = _inventoryDatabase.FindItemByID(itemEntity.ItemId);
                    InventoryItemConfig copyItem = Instantiate(item);
                
                    if (itemData is WeaponItemEntity weaponItemEntity)
                    {
                        if (copyItem is WeaponInventoryItemConfig weaponInventoryItemConfig)
                        {
                            weaponInventoryItemConfig.SetId(weaponItemEntity.EntityId);
                        
                            weaponInventoryItemConfig.SetCurrentAmmo(weaponItemEntity.CurrentAmmo);
                 
                            _inventory.AddItem(weaponInventoryItemConfig, true);
                        }
                    }
                    else
                    {
                        copyItem.ResetCount();
                        copyItem.AddCount(itemEntity.Count);
                        _inventory.AddItem(copyItem, true);
                    }
                }
            }
        }
    }
}