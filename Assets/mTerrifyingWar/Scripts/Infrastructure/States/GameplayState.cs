using UnityEngine;

public class GameplayState : IState
{
    private readonly GameStateMachine _stateMachine;
    private readonly IGameplayFactory _gameplayFactory;
    private readonly LoadingScreen _loadingScreen;
    private readonly PlayerSpawn playerSpawn;

    public GameplayState(GameStateMachine stateMachine, IGameplayFactory gameplayFactory, LoadingScreen loadingScreen, PlayerSpawn playerSpawn)
    {
        _stateMachine = stateMachine;
        _gameplayFactory = gameplayFactory;
        _loadingScreen = loadingScreen;
        this.playerSpawn = playerSpawn;
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
        _gameplayFactory.CreatePlayer(playerSpawn.transform);
        _gameplayFactory.CreateHud();
        _loadingScreen.Hide();
    }
}