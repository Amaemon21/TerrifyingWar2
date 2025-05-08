using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "MedicationsItem", menuName = "Inventory/MedicationsItem")]
public class MedicationsItemConfig : InventoryItemConfig
{
    [field: SerializeField, BoxGroup("Medications item config"), HorizontalLine] public float HealAmount { get; private set;}

    public override void Use(PlayerHealth player)
    {
        player.Heal(HealAmount);
    }
}