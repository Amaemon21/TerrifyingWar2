using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class Inventory : MonoBehaviour
{
    [Inject] private readonly DiContainer _container;
    [Inject] private readonly EventBus _eventBus;
    [Inject] private readonly InventoryManager _inventoryManager;
    
    private List<InventoryItemCell> _inventoryItemCells = new();

    [SerializeField, BoxGroup("Common"), HorizontalLine] private RectTransform _cellContainer;
    [SerializeField, BoxGroup("Common")] private int _cellCount = 36;
    [SerializeField, BoxGroup("Common")] private InventoryItemCell _cellPrefab;

    [SerializeField, BoxGroup("Weapons Cells"), HorizontalLine] private InventoryItemEquipableCell _primaryWeaponCell;
    [SerializeField, BoxGroup("Weapons Cells")] private InventoryItemEquipableCell _secondWeaponCell;

    private WeaponInventoryItemConfig _primaryWeapon;
    private WeaponInventoryItemConfig _secondWeapon;
    
    public WeaponInventoryItemConfig PrimaryWeapon => _primaryWeapon;
    public WeaponInventoryItemConfig SecondWeapon => _secondWeapon;

    public event Action RequestPrimaryWeaponChanged;
    public event Action RequestSecondWeaponChanged;

    private void Awake()
    {
        SpawnCells();

        DisplayItems();
    }

    private void OnEnable()
    {
        _primaryWeaponCell.DropItemChanged += UpdatePrimaryWeapon;
        _secondWeaponCell.DropItemChanged += UpdateSecondWeapon;
    }

    private void OnDisable()
    {
        _primaryWeaponCell.DropItemChanged -= UpdatePrimaryWeapon;
        _secondWeaponCell.DropItemChanged -= UpdateSecondWeapon;
    }

    public void DisplayItems()
    {
        foreach (var cell in _inventoryItemCells)
        {
            cell.RedrawCell();
        }
        
        _primaryWeaponCell.RedrawCell();
        _secondWeaponCell.RedrawCell();

        _eventBus.HandleDisplayItemsChanged();
    }
    
    public void AddItem(InventoryItemConfig config)
    {
        if (config.IsStackable)
            AddStackableItem(config);
        else
            AddUnstackableItem(config);

        DisplayItems();
    }

    private void AddStackableItem(InventoryItemConfig pickupedConfig)
    {
        foreach (InventoryItemCell item in _inventoryItemCells)
        {
            InventoryItemConfig config = item.InventoryItemConfig;
 
            if (config != null)
            {
                if (config.ItemID == pickupedConfig.ItemID)
                {
                    config.AddCount(pickupedConfig.ItemCount);
                    _eventBus.HandleItemAddedInventoryChanged();

                    return;
                }
            }
        }

        AddUnstackableItem(pickupedConfig);
    }
    
    private void AddUnstackableItem(InventoryItemConfig pickupedConfig)
    {
        for (var i = 0; i < _inventoryItemCells.Count; i++)
        {
            var item = _inventoryItemCells[i];
            
            InventoryItemConfig config = item.InventoryItemConfig;

            if (config == null)
            {
                item.SetItem(pickupedConfig);
                _eventBus.HandleItemAddedInventoryChanged();
                break;
            }
        }
    }

    public void RemoveItem(InventoryItemConfig config, InventoryItemCell specificCell = null)
    {
        if (specificCell != null)
        {
            if (TryRemoveFromCell(config, specificCell)) 
                return;
        }
        
        if (TryRemoveFromCell(config, _primaryWeaponCell))
        {
            _primaryWeapon = null;
            return;
        }

        if (TryRemoveFromCell(config, _secondWeaponCell))
        {
            _secondWeapon = null;
            return;
        }
        
        TryRemoveFromInventory(config);
    }

    private bool TryRemoveFromCell(InventoryItemConfig config, InventoryItemEquipableCell cell)
    {
        if (cell.InventoryItemConfig != null)
        {
            if (cell.InventoryItemConfig.ItemID == config.ItemID)
            {
                cell.SetItem(null);
                NotifyItemRemoved(config);
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
                NotifyItemRemoved(config);
                return true;
            }
        }
        
        return false;
    }

    private bool TryRemoveFromInventory(InventoryItemConfig config)
    {
        foreach (var itemCell in _inventoryItemCells)
        {
            if (itemCell.InventoryItemConfig != null)
            {
                if (itemCell.InventoryItemConfig.ItemID == config.ItemID)
                {                
                    itemCell.SetItem(null);
                    NotifyItemRemoved(config);
                    return true;
                }
            }
        }
        
        return false;
    }

    private void NotifyItemRemoved(InventoryItemConfig config)
    {
        _eventBus.HandleRemoveItemToInventoryChanged(config);
    }

    public void DropItem(InventoryItemConfig config, InventoryItemCell cell = null)
    {
        InventoryItemObject inventoryItemObject = Instantiate(config.ItemPrefab, _inventoryManager.DropPosition.transform.position, Quaternion.identity);

        inventoryItemObject.SetConfig(config);
        
        RemoveItem(config, cell);
    }

    public void DropItem(InventoryItemConfig config, int amount, InventoryItemCell cell = null)
    {
        InventoryItemObject inventoryItemObject = Instantiate(config.ItemPrefab, _inventoryManager.DropPosition.transform.position, Quaternion.identity);

        if (config.ItemCount < amount)
        {   
            config.RemoveCount(amount);
            
            var InventoryItemConfigCopy = Instantiate(config);
        
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

    private void UpdatePrimaryWeapon() => UpdateWeapon(_primaryWeaponCell, ref _primaryWeapon, RequestPrimaryWeaponChanged);

    private void UpdateSecondWeapon() => UpdateWeapon(_secondWeaponCell, ref _secondWeapon, RequestSecondWeaponChanged);

    private void UpdateWeapon(InventoryItemEquipableCell cell, ref WeaponInventoryItemConfig weaponSlot, Action onChanged)
    {
        if (cell.InventoryItemConfig is WeaponInventoryItemConfig weaponConfig && weaponConfig != weaponSlot)
        {
            weaponSlot = weaponConfig;
            onChanged?.Invoke();
        }
        else if (cell.InventoryItemConfig == null)
        {
            weaponSlot = null;
            onChanged?.Invoke();
        }
    }
    
    public AmmoInventoryItemConfig RequestAmmo(string ammoItemID)
    {
        return _inventoryItemCells
            .Select(cell => cell.InventoryItemConfig)
            .OfType<AmmoInventoryItemConfig>()
            .FirstOrDefault(ammo => ammo.ItemID == ammoItemID);
    }
    
    private void SpawnCells()
    {
        _inventoryItemCells.Clear();

        for (int i = 0; i < _cellCount; i++)
        {
            InventoryItemCell cell = _container.InstantiatePrefabForComponent<InventoryItemCell>(_cellPrefab, _cellContainer);
            cell.name = $"Slot: {i + 1}";
                        
            _inventoryItemCells.Add(cell);
        }
    }
}