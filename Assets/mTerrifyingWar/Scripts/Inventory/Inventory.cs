using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(InventoryComponent))]
[RequireComponent(typeof(InventoryCellFactory))]
public class Inventory : MonoBehaviour
{
    [Inject] private readonly PlayerProvider _playerProvider;
    [Inject] private readonly DisplayProvider _displayProvider;
    [Inject] private readonly BackendManager _backendManager;
    
    private readonly List<InventoryItemCell> _inventoryItemsCells = new();    
    
    private InventoryComponent _inventoryComponent;
    private InventoryCellFactory _inventoryCellFactory;
    
    public InventoryComponent InventoryComponent => _inventoryComponent;

    private void Awake()
    {
        _inventoryComponent = GetComponent<InventoryComponent>();
        
        _inventoryCellFactory = GetComponent<InventoryCellFactory>();
        _inventoryCellFactory.SpawnCells(_inventoryItemsCells);
        
        //_inventoryComponent.SetupDropPosition(_playerProvider.PlayerMover.GetComponentInChildren<DropPosition>());
        
        DisplayItems();
    }
    
    private void OnDisable()
    {
        _ = _backendManager.RemoveAllItemAsync();
    }    
    
    public void DisplayItems()
    {
        foreach (var cell in _inventoryItemsCells)
        {
            cell.RedrawCell();
        }
        
        _inventoryComponent.InventoryWeaponEquipable.DisplayItems();
    }
    
    public void AddItem(InventoryItemConfig config, bool isLoaded = false)
    {
        if (config.IsStackable)
            AddStackableItem(config, isLoaded);
        else
            AddUnstackableItem(config, isLoaded);
        
        DisplayItems();
    }

    public void AddEquipableItem(InventoryItemConfig config, SlotType slotType, bool isEquipped = false)
    {
        if (isEquipped)
        {
            if (config is WeaponInventoryItemConfig weaponInventoryItemConfig)
            {
                if (slotType == SlotType.Primary)
                {
                    _inventoryComponent.InventoryWeaponEquipable.PrimaryWeaponCell.SetItem(config);
                }
                else if (slotType == SlotType.Secondary)
                {
                    _inventoryComponent.InventoryWeaponEquipable.SecondaryWeaponCell.SetItem(config);
                }
            }
        }
        else
        {
            AddItem(config, true);
        }
    }

    private void AddStackableItem(InventoryItemConfig pickupedConfig, bool isLoaded = false)
    {
        foreach (InventoryItemCell item in _inventoryItemsCells)
        {
            InventoryItemConfig config = item.InventoryItemConfig;
 
            if (config != null)
            {
                if (config.ItemID == pickupedConfig.ItemID)
                {
                    if (!isLoaded)
                        _inventoryComponent.InventorySaver.AddItem(config);
                    
                    config.AddCount(pickupedConfig.ItemCount);
                    
                    _inventoryComponent.InventorySystem.HandleAddItem(config);
                    return;
                }
            }
        }

        AddUnstackableItem(pickupedConfig, isLoaded);
    }
    
    private void AddUnstackableItem(InventoryItemConfig pickupedConfig, bool isLoaded = false)
    {
        if (pickupedConfig is WeaponInventoryItemConfig weaponConfig)
        {
            if (TryAddWeaponToCell(_inventoryComponent.InventoryWeaponEquipable.PrimaryWeaponCell, weaponConfig, isLoaded) || 
                TryAddWeaponToCell(_inventoryComponent.InventoryWeaponEquipable.SecondaryWeaponCell, weaponConfig, isLoaded))
            {
                return;
            }
        }

        AddToFirstEmptyCell(pickupedConfig, isLoaded);
    }

    private bool TryAddWeaponToCell(InventoryItemEquipableCell cell, WeaponInventoryItemConfig weaponInventoryItemConfig, bool isLoaded)
    {
        if (cell.InventoryItemConfig == null)
        {
            if (!isLoaded)
                _inventoryComponent.InventorySaver.WeaponAddItem(weaponInventoryItemConfig, true);
            
            cell.SetItem(weaponInventoryItemConfig);
            return true;
        }
        
        return false;
    }

    private void AddToFirstEmptyCell(InventoryItemConfig config, bool isLoaded)
    {
        foreach (var cell in _inventoryItemsCells)
        {
            if (cell.InventoryItemConfig == null)
            {
                if (!isLoaded)
                    _inventoryComponent.InventorySaver.AddItem(config);
                
                cell.SetItem(config);
                _inventoryComponent.InventorySystem.HandleAddItem(config);
                break;
            }
        }
    }

    public void RemoveItem(InventoryItemConfig config, InventoryItemCell specificCell = null)
    {
        if (specificCell != null)
        {
            if (TryRemoveFromCell(config, specificCell))
            {
                _inventoryComponent.InventorySaver.RemoveItem(config);
                return;
            }
        }
        
        if (TryRemoveFromCell(config, _inventoryComponent.InventoryWeaponEquipable.PrimaryWeaponCell))
        {
            _inventoryComponent.InventoryWeaponEquipable.RemovePrimaryWeapon();
            _inventoryComponent.InventorySaver.RemoveItem(config);
            return;
        }

        if (TryRemoveFromCell(config, _inventoryComponent.InventoryWeaponEquipable.SecondaryWeaponCell))
        { 
            _inventoryComponent.InventoryWeaponEquipable.RemoveSecondaryWeapon();
            _inventoryComponent.InventorySaver.RemoveItem(config);
            return;
        }
        
        if (TryRemoveFromInventory(config))
        {
            _inventoryComponent.InventorySaver.RemoveItem(config);
        }
    }

    private bool TryRemoveFromCell(InventoryItemConfig config, InventoryItemEquipableCell cell)
    {
        if (cell.InventoryItemConfig != null)
        {
            if (cell.InventoryItemConfig.ItemID == config.ItemID)
            {
                cell.SetItem(null);
                _inventoryComponent.InventorySystem.HandleRemoveItem(config);
                return true;
            }
        }
        
        return false;
    }
    
    private bool TryRemoveFromCell(InventoryItemConfig config, InventoryItemCell cell)
    {
        if (cell.InventoryItemConfig != null)
        {
            if (cell.InventoryItemConfig.ItemID == config.ItemID)
            {
                cell.SetItem(null);
                _inventoryComponent.InventorySystem.HandleRemoveItem(config);
                return true;
            }
        }
        
        return false;
    }

    private bool TryRemoveFromInventory(InventoryItemConfig config)
    {
        foreach (var itemCell in _inventoryItemsCells)
        {
            if (itemCell.InventoryItemConfig != null)
            {
                if (itemCell.InventoryItemConfig.ItemID == config.ItemID)
                {                
                    itemCell.SetItem(null);
                    _inventoryComponent.InventorySystem.HandleRemoveItem(config);
                    return true;
                }
            }
        }
        
        return false;
    }

    public void DropItem(InventoryItemConfig config, InventoryItemCell cell = null)
    {
        InventoryItemObject inventoryItemObject = Instantiate(config.ItemPrefab, _inventoryComponent.DropPosition.transform.position, Quaternion.identity);

        inventoryItemObject.SetConfig(config);
        
        RemoveItem(config, cell);
    }

    public void DropItem(InventoryItemConfig config, int amount, InventoryItemCell cell = null)
    {
        InventoryItemObject inventoryItemObject = Instantiate(config.ItemPrefab, _inventoryComponent.DropPosition.transform.position, Quaternion.identity);

        if (config.ItemCount < amount)
        {   
            config.RemoveCount(amount);
            
            InventoryItemConfig InventoryItemConfigCopy = Instantiate(config);
        
            InventoryItemConfigCopy.ResetCount();
            InventoryItemConfigCopy.AddCount(amount);

            if (config.ItemCount <= 0)
                RemoveItem(config, cell);
            else
                inventoryItemObject.SetConfig(InventoryItemConfigCopy);
        }

        if (config.ItemCount == amount)
        {
            DropItem(config, cell);
        }
    }
}