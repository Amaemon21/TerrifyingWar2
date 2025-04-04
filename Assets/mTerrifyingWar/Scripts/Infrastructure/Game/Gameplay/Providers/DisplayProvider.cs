using UnityEngine;

public class DisplayProvider : MonoBehaviour
{
    [field: SerializeField] public Inventory Inventory { get; private set; }
    [field: SerializeField] public InventorySystem InventorySystem { get; private set; }
    [field: SerializeField] public AimPoint AimPoint { get; private set; }
    [field: SerializeField] public AmmoView AmmoView { get; private set; }
}