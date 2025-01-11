using System.Linq;
using UnityEngine;
using Zenject;

public class InventorySystem : MonoBehaviour
{
    [Inject] private readonly Inventory _inventory;
    
    public AmmoInventoryItemConfig RequestAmmo(string ammoItemID)
    {
        return _inventory.InventoryItemCell
            .Select(cell => cell.InventoryItemConfig)
            .OfType<AmmoInventoryItemConfig>()
            .FirstOrDefault(ammo => ammo.ItemID == ammoItemID);
    }
}