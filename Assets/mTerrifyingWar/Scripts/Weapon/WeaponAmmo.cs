using UnityEngine;
using Zenject;

public class WeaponAmmo : MonoBehaviour
{
    [Inject] private readonly DisplayProvider _displayProvider;

    [SerializeField] private Weapon _weapon;
    [SerializeField] private WeaponAnimator _weaponAnimator;
    [SerializeField] private WeaponSaver _weaponSaver;
    
    private AmmoInventoryItemConfig _ammoInventoryItemConfig;
    
    public bool IsReloading {get; private set;}

    public void OnEnable()
    {
        _displayProvider.Inventory.InventorySystem.OnAmmoAddChanged += RequestAmmo;
        _displayProvider.Inventory.InventorySystem.OnAmmoRemoveChanged += RemoveAmmo;
        
        RequestAmmo();
    }
    
    private void OnDisable()
    {
        _displayProvider.Inventory.InventorySystem.OnAmmoAddChanged -= RequestAmmo;
        _displayProvider.Inventory.InventorySystem.OnAmmoRemoveChanged -= RemoveAmmo;
    }
    
    public void OnReload()
    {
        if (_ammoInventoryItemConfig == null)
            return;

        if (_ammoInventoryItemConfig.ItemCount <= 0 || _weapon.WeaponInventoryItemConfig.CurrentAmmo >= _weapon.WeaponInventoryItemConfig.MagazineSize)
            return;

        _weaponAnimator.PlayReload();

        float delay = _weapon.WeaponInventoryItemConfig.CurrentAmmo == 0 ? _weapon.WeaponAnimator.EmptyReloadDelay : _weapon.WeaponAnimator.TacReloadDelay;
        
        Invoke(nameof(AddAmmo), delay);
        
        IsReloading = true;
    }
    
    protected void AddAmmo()
    {
        if (_ammoInventoryItemConfig == null)
            return;

        int amountNeeded = _weapon.WeaponInventoryItemConfig.MagazineSize - _weapon.WeaponInventoryItemConfig.CurrentAmmo;

        if (amountNeeded >= _ammoInventoryItemConfig.ItemCount)
        {
            int residue = _ammoInventoryItemConfig.ItemCount;
            
            _weapon.WeaponInventoryItemConfig.AddCurrentAmmo(_ammoInventoryItemConfig.ItemCount);
            _ammoInventoryItemConfig.RemoveCount(_ammoInventoryItemConfig.ItemCount);
            _displayProvider.Inventory.InventorySaver.RemoveItem(_ammoInventoryItemConfig, residue);
        }
        else
        {
            _weapon.WeaponInventoryItemConfig.SetCurrentAmmo();
            _ammoInventoryItemConfig.RemoveCount(amountNeeded);
            _displayProvider.Inventory.InventorySaver.RemoveItem(_ammoInventoryItemConfig, amountNeeded);
        }

        _weaponSaver.WeaponItemEntity.CurrentAmmo = _weapon.WeaponInventoryItemConfig.CurrentAmmo;
        HandleDisplayAmmo();
        
        _weapon.ChangeCanFire(true);
        IsReloading = false;
    }
    
    public void ResetAvailableAmmo()
    {
        if (_ammoInventoryItemConfig != null)
        {
            _ammoInventoryItemConfig.ResetCount();
            _displayProvider.Inventory.RemoveItem(_ammoInventoryItemConfig);
        }
    }

    private void RequestAmmo()
    {
        if (_weapon.WeaponInventoryItemConfig == null)
            return;
        
        _ammoInventoryItemConfig = _displayProvider.Inventory.InventorySystem.RequestAmmo(_weapon.WeaponInventoryItemConfig.EAmmoType);
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
        if (_weapon == null)
            return;
        
        if (_weapon.WeaponInventoryItemConfig == null)
            return;

        int ammoCount = 0;
        
        if (_ammoInventoryItemConfig != null)
        {
            ammoCount = _ammoInventoryItemConfig.ItemCount;
        }
        
        _displayProvider.AmmoView.DisplayAmmo(_weapon.WeaponInventoryItemConfig.CurrentAmmo, ammoCount, _weapon);
    }
}