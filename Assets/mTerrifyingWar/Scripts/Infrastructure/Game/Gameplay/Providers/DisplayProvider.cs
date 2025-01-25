public class DisplayProvider
{
    public Inventory Inventory { get; private set; }
    public InventorySystem InventorySystem { get; private set; }
    public AimPoint AimPoint { get; private set; }
    public AmmoView AmmoView { get; private set; }

    public void Setup(DisplayContainer displayContainer)
    {
        Inventory = displayContainer.Inventory;
        InventorySystem = displayContainer.InventorySystem;
        AimPoint = displayContainer.AimPoint;
        AmmoView = displayContainer.AmmoView;
    }
}