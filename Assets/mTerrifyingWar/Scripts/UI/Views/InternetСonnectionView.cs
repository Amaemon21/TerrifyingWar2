using System.Collections;
using UnityEngine;
using Zenject;

public class InternetConnectionView : MonoBehaviour
{
    [Inject] private readonly UIWindowService _windowService;
    
    [SerializeField] private NotificationSystem _notificationSystem;
    [SerializeField] private InternetAccess _internetAccess;

    private void Start()
    {
        StartCoroutine(StartConnection());
    }
    
    private IEnumerator StartConnection()
    {
        _notificationSystem.AddMessage("Проверка подключения к интернету", Color.white);

        yield return new WaitForSeconds(2f);
        
        StartCoroutine(_internetAccess.TestConnection(UpdateConnectionStatus));
    }

    private void UpdateConnectionStatus(bool isConnected)
    {
        if (isConnected)
        {
            _notificationSystem.AddMessage("Подключение успешно", Color.green);
            _windowService.OpenWindow(WindowType.Authorization);
        }
        else
        {
            _notificationSystem.AddMessage("Ошибка подключения", Color.red);
        }
    }
}