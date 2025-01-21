using UnityEngine;
using Zenject;

public class LoadGameButton : MonoBehaviour
{
    [Inject] private readonly GameStateMachine _gameStateMachine;

    public void Click()
    {
        _gameStateMachine.Enter<LoadGameplayState>();
    }
}