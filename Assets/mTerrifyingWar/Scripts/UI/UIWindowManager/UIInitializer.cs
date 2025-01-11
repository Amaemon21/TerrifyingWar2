using UnityEngine;
using Zenject;

public class UIInitializer : MonoBehaviour
{
    [Inject] private readonly UIWindowService _uiWindowService;
    
    [SerializeField] private UIPause _pause;
    [SerializeField] private UIInventory _inventory;
    [SerializeField] private UIMap _map;

    private void Awake()
    {
        _uiWindowService.SubscribeWindow(WindowType.Pause, _pause);
        _uiWindowService.SubscribeWindow(WindowType.Inventory, _inventory);
        _uiWindowService.SubscribeWindow(WindowType.Map, _map);
    }
}