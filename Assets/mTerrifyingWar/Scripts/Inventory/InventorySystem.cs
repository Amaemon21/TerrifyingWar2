using System.Linq;
using UnityEngine;
using Zenject;

public class InventorySystem : MonoBehaviour
{
    [Inject] private readonly DisplayProvider _displayProvider;
    
    public AmmoInventoryItemConfig RequestAmmo(EAmmoType eAmmoType)
    {
        return _displayProvider.Inventory.InventoryItemCell
            .Select(cell => cell.InventoryItemConfig)
            .OfType<AmmoInventoryItemConfig>()
            .FirstOrDefault(ammo => ammo.EAmmoType == eAmmoType);
    }
}