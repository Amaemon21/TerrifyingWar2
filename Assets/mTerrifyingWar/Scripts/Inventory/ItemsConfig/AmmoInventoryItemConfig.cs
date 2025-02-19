using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "AmmoItem", menuName = "Inventory/AmmoItem")]
public class AmmoInventoryItemConfig : InventoryItemConfig
{
    [field: SerializeField, BoxGroup("Ammo item config"), HorizontalLine] public EAmmoType EAmmoType { get; private set;}
}