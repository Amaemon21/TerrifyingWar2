using UnityEngine;
using Zenject;

public class LoadMainMenuButton : MonoBehaviour
{
    [Inject] private GameEntryPoint _gameEntryPoint;

    public void HandleMainMenuLoad()
    {
        _gameEntryPoint.LoadMainMenu();
    }
}