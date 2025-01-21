using UnityEngine;
using Zenject;

public class LoadMainMenuButton : MonoBehaviour
{
    [Inject] private readonly GameStateMachine _gameStateMachine;

    public void Click()
    {
        _gameStateMachine.Enter<LoadMainMenuState, IExitableState>(_gameStateMachine.GetActiveState());
    }
}