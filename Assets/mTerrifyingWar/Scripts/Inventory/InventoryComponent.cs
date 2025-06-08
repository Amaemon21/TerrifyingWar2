using NaughtyAttributes;
using UnityEngine;

public class InventoryComponent : MonoBehaviour
{
    [field: SerializeField, BoxGroup("Component"), HorizontalLine] public Inventory Inventory { get; private set;}
    [field: SerializeField, BoxGroup("Component")] public InventorySystem InventorySystem { get; private set;}
    [field: SerializeField, BoxGroup("Component")] public InventoryCellFactory InventoryCellFactory { get; private set;}
    [field: SerializeField, BoxGroup("Component")] public InventoryWeaponEquipable InventoryWeaponEquipable { get; private set;}
    [field: SerializeField, BoxGroup("Component")] public InventorySaver InventorySaver { get; private set;}
    [field: SerializeField, BoxGroup("Component")] public InventoryDragableObject DragableObject { get; private set;}
    [field: SerializeField, BoxGroup("Component")] public DropArea DropArea { get; private set;}
    [field: SerializeField, BoxGroup("Component")] public ItemInfoView ItemInfoView { get; private set;}
    [field: SerializeField, BoxGroup("Component")] public ActionMenuObject ActionMenuObject { get; private set;}
    [field: SerializeField, BoxGroup("Component")] public DropMenu DropMenu { get; private set;}
    [field: SerializeField, BoxGroup("Component")] public LootSloots LootSlots { get; private set;}
    
    public DropPosition DropPosition { get; private set;}
    public InventoryDatabase InventoryDatabase { get; private set;}

    private void Awake()
    {
        InventoryDatabase = Resources.Load<InventoryDatabase>(AssetsPath.InventoryDatabasePath);

        Inventory.Setup(this);
        DropArea.Setup();
        ActionMenuObject.Setup(this);
        DropMenu.Setup();
    }

    public void SetupDropPosition(DropPosition dropPosition)
    {
        DropPosition = dropPosition;
    }
}