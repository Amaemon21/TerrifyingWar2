using UnityEngine;

public class GameplayState : IState
{
    private readonly GameStateMachine _stateMachine;
    private readonly IGameplayFactory _gameplayFactory;
    private readonly LoadingScreen _loadingScreen;
    private readonly SpawnPlayerPoint _spawnPlayerPoint;

    public GameplayState(GameStateMachine stateMachine, IGameplayFactory gameplayFactory, LoadingScreen loadingScreen, SpawnPlayerPoint spawnPlayerPoint)
    {
        _stateMachine = stateMachine;
        _gameplayFactory = gameplayFactory;
        _loadingScreen = loadingScreen;
        _spawnPlayerPoint = spawnPlayerPoint;
    }

    public void Enter()
    {        
        SetupWorld();
    }

    public void Exit()
    {
    }

    private void SetupWorld()
    {
        _gameplayFactory.CreatePlayer(_spawnPlayerPoint.transform);
        _gameplayFactory.CreateHud();
        _loadingScreen.Hide();
    }
}