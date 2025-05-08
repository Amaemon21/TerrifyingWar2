using UnityEngine;

public class DisplayProvider
{
    public Canvas Canvas { get; private set; }
    public Inventory Inventory { get; private set; }
    public AimPoint AimPoint { get; private set; }
    public AmmoView AmmoView { get; private set; }
    public NotificationSystem NotificationSystem { get; private set; }
    
    public void Setup(DisplayContainer displayContainer)
    {
        Canvas = displayContainer.Canvas;
        Inventory = displayContainer.Inventory;
        AimPoint = displayContainer.AimPoint;
        AmmoView = displayContainer.AmmoView;
        NotificationSystem = displayContainer.NotificationSystem;
    }
}