using System;
using NaughtyAttributes;
using UnityEngine;

public class InventoryWeaponEquipable : MonoBehaviour
{
    [field: SerializeField, BoxGroup("Weapons Cells"), HorizontalLine] public InventoryItemEquipableCell PrimaryWeaponCell { get; private set; }
    [field: SerializeField, BoxGroup("Weapons Cells")] public InventoryItemEquipableCell SecondaryWeaponCell { get; private set; }

    private WeaponInventoryItemConfig _primaryWeapon;
    private WeaponInventoryItemConfig _secondaryWeapon;

    public WeaponInventoryItemConfig PrimaryWeapon => _primaryWeapon;
    public WeaponInventoryItemConfig SecondaryWeapon => _secondaryWeapon;
    
    public event Action RequestPrimaryWeaponChanged;
    public event Action RequestSecondWeaponChanged;
    
    private void OnEnable()
    {
        PrimaryWeaponCell.DropItemChanged += UpdatePrimaryWeapon;
        SecondaryWeaponCell.DropItemChanged += UpdateSecondWeapon;
    }

    private void OnDisable()
    {
        PrimaryWeaponCell.DropItemChanged -= UpdatePrimaryWeapon;
        SecondaryWeaponCell.DropItemChanged -= UpdateSecondWeapon;
    }

    public void DisplayItems()
    {
        PrimaryWeaponCell.RedrawCell();
        SecondaryWeaponCell.RedrawCell();
    }

    public void RemovePrimaryWeapon()
    {
        _primaryWeapon = null;   
    }
    
    public void RemoveSecondaryWeapon()
    {
        _secondaryWeapon = null;
    }    
    
    private void UpdatePrimaryWeapon()
    {
        UpdateWeapon(PrimaryWeaponCell, ref _primaryWeapon, RequestPrimaryWeaponChanged);
    }

    private void UpdateSecondWeapon()
    {
        UpdateWeapon(SecondaryWeaponCell, ref _secondaryWeapon, RequestSecondWeaponChanged);
    }

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