public class DisplayProvider
{
    public Inventory Inventory { get; private set; }
    public InventorySystem InventorySystem { get; private set; }
    public AimPoint AimPoint { get; private set; }
    public AmmoView AmmoView { get; private set; }

    public void Setup(Inventory inventory, InventorySystem inventorySystem, AimPoint aimPoint, AmmoView ammoView)
    {
        Inventory = inventory;
        InventorySystem = inventorySystem;
        AimPoint = aimPoint;
        AmmoView = ammoView;
    }
}