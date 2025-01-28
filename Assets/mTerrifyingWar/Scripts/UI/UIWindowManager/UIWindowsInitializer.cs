using UnityEngine;
using Zenject;

public class UIWindowsInitializer : MonoBehaviour
{
    [Inject] private readonly UIWindowService _uiWindowService;

    [SerializeField] private UIPause _pause;
    [SerializeField] private UIInventory _inventory;
    [SerializeField] private UIMap _map;
    [SerializeField] private UIGameEnd _gameEnd;

    private void OnEnable()
    {
        SubscribeWindows();
    }
    
    private void OnDisable()
    {
        UnsubscribeWindows();
    }

    private void SubscribeWindows()
    {
        _uiWindowService.SubscribeWindow(WindowType.Pause, _pause);
        _uiWindowService.SubscribeWindow(WindowType.Inventory, _inventory);
        _uiWindowService.SubscribeWindow(WindowType.Map, _map);
        _uiWindowService.SubscribeWindow(WindowType.GameEnd, _gameEnd);
    }

    private void UnsubscribeWindows()
    {
        _uiWindowService.UnsubscribeWindow(WindowType.Pause);
        _uiWindowService.UnsubscribeWindow(WindowType.Inventory);
        _uiWindowService.UnsubscribeWindow(WindowType.Map);
        _uiWindowService.UnsubscribeWindow(WindowType.GameEnd);
    }
}