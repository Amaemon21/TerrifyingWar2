using System.Collections;
using UnityEngine;

public class GameplayState : IState
{
    private readonly GameStateMachine _stateMachine;
    private readonly IGameplayFactory _gameplayFactory;
    private readonly LoadingScreen _loadingScreen;
    private readonly Coroutines _coroutines;

    public GameplayState(GameStateMachine stateMachine, IGameplayFactory gameplayFactory, LoadingScreen loadingScreen, Coroutines coroutine)
    {
        _stateMachine = stateMachine;
        _gameplayFactory = gameplayFactory;
        _loadingScreen = loadingScreen;
        _coroutines = coroutine;
    }

    public void Enter()
    {
        _coroutines.StartCoroutine(SetupWorld());
    }

    public void Exit()
    {
    }
    
    private IEnumerator SetupWorld()
    {
        var playerSpawnPosition = Object.FindFirstObjectByType<PlayerSpawnPosition>();
        
        _gameplayFactory.CreatePlayer(playerSpawnPosition.transform);
        _gameplayFactory.CreateHud();
        
        yield return null;
        
        _stateMachine.Enter<GameloopState>();
        
        yield return null;
        
        _loadingScreen.Hide();
    }


}