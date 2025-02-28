using UnityEngine;
using Zenject;

public class InternetConnectionView : MonoBehaviour
{
    [Inject] private readonly UIWindowService _windowService;
    
    [SerializeField] private NotificationSystem _notificationSystem;
    [SerializeField] private InternetAccess _internetAccess;

    private void Start()
    {
        _notificationSystem.AddMessage("Проверка подключения к интернету");
        
        StartCoroutine(_internetAccess.TestConnection(UpdateConnectionStatus));
    }

    private void UpdateConnectionStatus(bool isConnected)
    {
        if (isConnected)
        {
            _notificationSystem.AddMessage("Подключение успешно");
            StartCoroutine(_windowService.OpenWindowWithDelay(WindowType.Authorization, 3f));
        }
        else
        {
            _notificationSystem.AddMessage("Ошибка подключения");
        }
    }
}