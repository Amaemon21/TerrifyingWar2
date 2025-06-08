using UnityEngine;

public class DisplayProvider
{
    public Canvas Canvas { get; private set; }
    public InventoryComponent InventoryComponent { get; private set; }
    public AimPoint AimPoint { get; private set; }
    public AmmoView AmmoView { get; private set; }
    public NotificationSystem NotificationSystem { get; private set; }
    public WorkbenchSystem WorkbenchSystem { get; private set; }
    
    public void Setup(DisplayContainer displayContainer)
    {
        Canvas = displayContainer.Canvas;
        InventoryComponent = displayContainer.InventoryComponent;
        AimPoint = displayContainer.AimPoint;
        AmmoView = displayContainer.AmmoView;
        NotificationSystem = displayContainer.NotificationSystem;
        WorkbenchSystem = displayContainer.WorkbenchSystem;
    }
}