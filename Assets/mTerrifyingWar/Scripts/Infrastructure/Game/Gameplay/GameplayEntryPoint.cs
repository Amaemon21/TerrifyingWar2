using UnityEngine;
using Zenject;

public class GameplayEntryPoint : MonoBehaviour
{
    [Inject] private readonly GameStateMachine _gameStateMachine;
    [Inject] private readonly DiContainer _container;

    private GameplayState _gameplayState;
    
    public void Run()
    {
        _gameplayState = _container.Instantiate<GameplayState>();
        _gameStateMachine.AddState(_gameplayState);
        _gameStateMachine.Enter<GameplayState>();
    }

    private void OnDestroy()
    {
        _gameStateMachine.RemoveState(_gameplayState);
    }
}