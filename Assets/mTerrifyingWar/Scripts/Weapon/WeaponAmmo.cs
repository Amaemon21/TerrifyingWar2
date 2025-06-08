using System;
using UnityEngine;
using Zenject;

public class WeaponAmmo : MonoBehaviour
{
    [Inject] private readonly IStorageService _storageService;
    [Inject] private readonly PlayerProvider _playerProvider;
    [Inject] private readonly DisplayProvider _displayProvider;

    [SerializeField] private Weapon _weapon;
    [SerializeField] private WeaponAnimator _weaponAnimator;
    
    private AmmoInventoryItemConfig _ammoInventoryItemConfig;
    
    public bool IsReloading {get; private set;}

    public void OnEnable()
    {
        _displayProvider.InventoryComponent.InventorySystem.OnAmmoAddChanged += RequestAmmo;
        _displayProvider.InventoryComponent.InventorySystem.OnAmmoRemoveChanged += RemoveAmmo;
        
        RequestAmmo();
    }
    
    private void OnDisable()
    {
        _displayProvider.InventoryComponent.InventorySystem.OnAmmoAddChanged -= RequestAmmo;
        _displayProvider.InventoryComponent.InventorySystem.OnAmmoRemoveChanged -= RemoveAmmo;
    }
    
    public void OnReload()
    {
        if (!_weaponAnimator.IsEquipped) 
            return;
        
        if (_ammoInventoryItemConfig == null)
            return;

        if (_ammoInventoryItemConfig.ItemCount <= 0 || _weapon.WeaponInventoryItemConfig.CurrentAmmo >= _weapon.WeaponInventoryItemConfig.MagazineSize)
            return;

        _weaponAnimator.PlayReload();

        float delay = _weapon.WeaponInventoryItemConfig.CurrentAmmo == 0 ? _weapon.WeaponAnimator.EmptyReloadDelay : _weapon.WeaponAnimator.TacReloadDelay;
        
        Invoke(nameof(AddAmmo), delay);
        _playerProvider.UIBluer.ActiveBluer();
        IsReloading = true;
    }
    
    protected void AddAmmo()
    {
        if (_ammoInventoryItemConfig == null)
            return;

        int amountNeeded = _weapon.WeaponInventoryItemConfig.MagazineSize - _weapon.WeaponInventoryItemConfig.CurrentAmmo;
        int amountToRemove = Math.Min(amountNeeded, _ammoInventoryItemConfig.ItemCount);
        
        _weapon.WeaponInventoryItemConfig.AddCurrentAmmo(amountToRemove);
        
        _ammoInventoryItemConfig.RemoveCount(amountToRemove);
        _displayProvider.InventoryComponent.InventorySaver.RemoveItem(_ammoInventoryItemConfig, amountToRemove);
        
        _weapon.WeaponInventoryItemConfig.WeaponItemEntity.CurrentAmmo = _weapon.WeaponInventoryItemConfig.CurrentAmmo;
        
        HandleDisplayAmmo();
        
        _weapon.ChangeCanFire(true);
        _playerProvider.UIBluer.DeactiveBluer();
        IsReloading = false;
    }
    
    public void ResetAvailableAmmo()
    {
        if (_ammoInventoryItemConfig != null)
        {
            _ammoInventoryItemConfig.ResetCount();
            _displayProvider.InventoryComponent.Inventory.RemoveItem(_ammoInventoryItemConfig);
        }
    }

    private void RequestAmmo()
    {
        if (_weapon.WeaponInventoryItemConfig == null)
            return;
        
        _ammoInventoryItemConfig = _displayProvider.InventoryComponent.InventorySystem.RequestAmmo(_weapon.WeaponInventoryItemConfig.EAmmoType);
        HandleDisplayAmmo();
    }

    private void RemoveAmmo()
    {
        if (_ammoInventoryItemConfig.EAmmoType == _weapon.WeaponInventoryItemConfig.EAmmoType)
        {
            _ammoInventoryItemConfig = null;
            HandleDisplayAmmo();
        }
    }
    
    public void HandleDisplayAmmo()
    {
        if (_weapon?.WeaponInventoryItemConfig == null)
            return;

        int reserveAmmo = _ammoInventoryItemConfig?.ItemCount ?? 0;
        int currentAmmo = _weapon.WeaponInventoryItemConfig.CurrentAmmo;

        _displayProvider.AmmoView.DisplayAmmo(currentAmmo, reserveAmmo, _weapon);
    }
}