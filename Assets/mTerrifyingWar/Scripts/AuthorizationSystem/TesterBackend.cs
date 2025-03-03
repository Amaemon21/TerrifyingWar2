using UnityEngine;
using Zenject;

public class TesterBackend : MonoBehaviour
{
    [Inject] private readonly BackendManager backendManager;
    
    private async void Start()
    {
        await backendManager.AddItemAsync(backendManager.PlayerId, backendManager.PlayerName, 101, 5);
    }
}