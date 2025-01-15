using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class Inventory : MonoBehaviour
{
    [Inject] private readonly PlayerProvider _playerProvider;
    
    [field: SerializeField, BoxGroup("Database"), HorizontalLine] public InventoryDatabase InventoryDatabase { get; private set;}
    [field: SerializeField, BoxGroup("Utils"), HorizontalLine] public InventoryDragableObject DragableObject { get; private set;}
    [field: SerializeField, BoxGroup("Utils")] public DropArea DropArea { get; private set;}
    [field: SerializeField, BoxGroup("Utils")] public ItemInfo ItemInfo { get; private set;}
    [field: SerializeField, BoxGroup("Utils")] public ActionMenuObject ActionMenuObject { get; private set;}
    [field: SerializeField, BoxGroup("Utils")] public DropMenu DropMenu { get; private set;}
    public DropPosition DropPosition { get; private set;}
    
    [SerializeField, BoxGroup("Weapons Cells"), HorizontalLine] private InventoryItemEquipableCell _primaryWeaponCell;
    [SerializeField, BoxGroup("Weapons Cells")] private InventoryItemEquipableCell _secondWeaponCell;

    private List<InventoryItemCell> _inventoryItemsCells = new();

    private InventoryCellFactory _inventoryCellFactory;

    private WeaponInventoryItemConfig _primaryWeapon;
    private WeaponInventoryItemConfig _secondWeapon;

    public List<InventoryItemCell> InventoryItemCell => _inventoryItemsCells;
    
    public WeaponInventoryItemConfig PrimaryWeapon => _primaryWeapon;
    public WeaponInventoryItemConfig SecondWeapon => _secondWeapon;

    public event Action RequestPrimaryWeaponChanged;
    public event Action RequestSecondWeaponChanged;
    public event Action ItemAddedInventoryChanged;
    public event Action<InventoryItemConfig> ItemRemoveInventoryChanged;

    private void Awake()
    {
        _inventoryCellFactory = GetComponent<InventoryCellFactory>();
        _inventoryCellFactory.SpawnCells(_inventoryItemsCells);
        
        DropPosition = _playerProvider.PlayerController.GetComponentInChildren<DropPosition>();
        
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
        foreach (var cell in _inventoryItemsCells)
        {
            cell.RedrawCell();
        }
        
        _primaryWeaponCell.RedrawCell();
        _secondWeaponCell.RedrawCell();
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
        foreach (InventoryItemCell item in _inventoryItemsCells)
        {
            InventoryItemConfig config = item.InventoryItemConfig;
 
            if (config != null)
            {
                if (config.ItemID == pickupedConfig.ItemID)
                {
                    config.AddCount(pickupedConfig.ItemCount);
                    ItemAddedInventoryChanged?.Invoke();

                    return;
                }
            }
        }

        AddUnstackableItem(pickupedConfig);
    }
    
    private void AddUnstackableItem(InventoryItemConfig pickupedConfig)
    {
        for (var i = 0; i < _inventoryItemsCells.Count; i++)
        {
            var item = _inventoryItemsCells[i];
            
            InventoryItemConfig config = item.InventoryItemConfig;

            if (config == null)
            {
                item.SetItem(pickupedConfig);
                ItemAddedInventoryChanged?.Invoke();
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
                ItemRemoveInventoryChanged?.Invoke(config);
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
                ItemRemoveInventoryChanged?.Invoke(config);
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
                    ItemRemoveInventoryChanged?.Invoke(config);
                    return true;
                }
            }
        }
        
        return false;
    }

    public void DropItem(InventoryItemConfig config, InventoryItemCell cell = null)
    {
        InventoryItemObject inventoryItemObject = Instantiate(config.ItemPrefab, DropPosition.transform.position, Quaternion.identity);

        inventoryItemObject.SetConfig(config);
        
        RemoveItem(config, cell);
    }

    public void DropItem(InventoryItemConfig config, int amount, InventoryItemCell cell = null)
    {
        InventoryItemObject inventoryItemObject = Instantiate(config.ItemPrefab, DropPosition.transform.position, Quaternion.identity);

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
}