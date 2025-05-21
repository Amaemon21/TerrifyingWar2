using UnityEngine;

public class DisplayContainer : MonoBehaviour
{
    [field: SerializeField] public Canvas Canvas { get; private set; }
    [field: SerializeField] public Inventory Inventory { get; private set; }
    [field: SerializeField] public AimPoint AimPoint { get; private set; }
    [field: SerializeField] public AmmoView AmmoView { get; private set; }
    [field: SerializeField] public NotificationSystem NotificationSystem { get; private set; }
}
