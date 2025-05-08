using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "ScopeItem", menuName = "Inventory/ScopeItem")]
public class ScopeInventoryItemConfig : InventoryItemConfig
{
    [field: SerializeField, BoxGroup("Ammo item config"), HorizontalLine] public Scope Scope { get; private set;}
    [field: SerializeField, BoxGroup("Ammo item config")] public Vector3 Position { get; private set;}
}