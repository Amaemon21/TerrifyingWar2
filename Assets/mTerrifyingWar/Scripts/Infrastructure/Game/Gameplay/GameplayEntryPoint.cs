using UnityEngine;
using Zenject;

public class GameplayEntryPoint : MonoBehaviour
{
    [Inject] private readonly IInputService _inputService;
    [Inject] private readonly BackendManager _backendManager;
    [Inject] private readonly GameStateMachine _gameStateMachine;
    [Inject] private readonly DiContainer _container;
    
    public void Run()
    {
        LevelState levelState = _container.Instantiate<LevelState>();

        _gameStateMachine.AddState(levelState);
        _gameStateMachine.Enter<LevelState>();
        
        _inputService.EnablePlayerMap();
        
        _backendManager.SendPlayerProgressAsync(new PlayerProgress(_backendManager.PlayerId, 666, 100, 1, 10));
    }
}