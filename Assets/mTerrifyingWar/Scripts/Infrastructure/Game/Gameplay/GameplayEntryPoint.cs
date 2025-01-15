using UnityEngine;
using Zenject;

public class GameplayEntryPoint : MonoBehaviour
{
    [Inject] private readonly GameStateMachine _gameStateMachine;
    [Inject] private readonly DiContainer _container;
    
    public void Run()
    {
        GameplayState gameplayState = _container.Instantiate<GameplayState>();

        _gameStateMachine.AddState(gameplayState);
        _gameStateMachine.Enter<GameplayState>();
    }
}