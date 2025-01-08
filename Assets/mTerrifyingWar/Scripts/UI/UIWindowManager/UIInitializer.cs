using UnityEngine;
using Zenject;

public class UIInitializer : MonoBehaviour
{
    [Inject] private readonly UIManager _uiManager;
    
    [SerializeField] private UIPause _pause;
    [SerializeField] private UIInventory _inventory;
    [SerializeField] private UIMap _map;

    private void Awake()
    {
        _uiManager.SubscribeWindow(WindowType.Pause, _pause);
        _uiManager.SubscribeWindow(WindowType.Inventory, _inventory);
        _uiManager.SubscribeWindow(WindowType.Map, _map);
    }
}