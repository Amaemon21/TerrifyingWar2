using UnityEngine;

public class GameplayState : IState
{
    private readonly GameStateMachine _stateMachine;
    private readonly IGameplayFactory _gameplayFactory;
    private readonly LoadingScreen _loadingScreen;
    private readonly Vector3 _playerSpawnPosition;

    public GameplayState(GameStateMachine stateMachine, IGameplayFactory gameplayFactory, LoadingScreen loadingScreen, PlayerSpawn playerSpawn)
    {
        _stateMachine = stateMachine;
        _gameplayFactory = gameplayFactory;
        _loadingScreen = loadingScreen;
        _playerSpawnPosition = playerSpawn.transform.position;
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
        _gameplayFactory.CreatePlayer(_playerSpawnPosition);
        _gameplayFactory.CreateHud();
        _loadingScreen.Hide();
    }
}