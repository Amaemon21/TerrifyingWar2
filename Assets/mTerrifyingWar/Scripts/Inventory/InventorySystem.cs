using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class InventorySystem : MonoBehaviour
{
    [Inject] private readonly DisplayProvider _displayProvider;
    
    private List<WeaponInventoryItemConfig> _weapons = new();
    private List<AmmoInventoryItemConfig> _ammoList = new();
    
    public event Action OnAmmoAddChanged; 
    public event Action OnAmmoRemoveChanged; 
    
    public void HandleAddItem(InventoryItemConfig config)
    {
        if (config is AmmoInventoryItemConfig ammo)
        {
            if (!_ammoList.Contains(ammo))
                _ammoList.Add(ammo);
        }
        else if (config is WeaponInventoryItemConfig weapon)
        {
            if (!_weapons.Contains(weapon))
                _weapons.Add(weapon);
        }
        
        OnAmmoAddChanged?.Invoke();
    }

    public void HandleRemoveItem(InventoryItemConfig config)
    {
        if (config is AmmoInventoryItemConfig ammo)
        {
            _ammoList.Remove(ammo);
        }
        else if (config is WeaponInventoryItemConfig weapon)
        {
            _weapons.Remove(weapon);
        }
        
        OnAmmoRemoveChanged?.Invoke();
    }
    
    public AmmoInventoryItemConfig RequestAmmo(EAmmoType type)
    {
        return _ammoList.FirstOrDefault(ammo => ammo.EAmmoType == type);
    }
}