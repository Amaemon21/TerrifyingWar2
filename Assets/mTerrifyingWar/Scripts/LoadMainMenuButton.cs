using UnityEngine;
using Zenject;

public class LoadMainMenuButton : MonoBehaviour
{
    [Inject] private GameStateMachine _gameStateMachine;

    public void HandleMainMenuLoad()
    {
        _gameStateMachine.Enter<LoadMainMenuState>();
    }
}