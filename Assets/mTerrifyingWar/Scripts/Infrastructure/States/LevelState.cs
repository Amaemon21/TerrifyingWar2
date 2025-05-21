using UnityEngine;

public class LevelState : IState
{
    private readonly GameStateMachine _stateMachine;
    private readonly IGameFactory _gameFactory;
    private readonly IGameplayFactory _gameplayFactory;
    private readonly LoadingScreen _loadingScreen;
    private readonly IPersistentProgressService _progressService;

    public LevelState(GameStateMachine stateMachine, IGameFactory gameFactory, IGameplayFactory gameplayFactory, LoadingScreen loadingScreen, IPersistentProgressService progressService)
    {
        _stateMachine = stateMachine;
        _gameFactory = gameFactory;
        _gameplayFactory = gameplayFactory;
        _loadingScreen = loadingScreen;
        _progressService = progressService;
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
        PlayerSpawnPosition playerSpawnPosition = Object.FindFirstObjectByType<PlayerSpawnPosition>();
        
        _gameplayFactory.CreatePlayer(playerSpawnPosition.transform);
        _gameplayFactory.CreateHud();
        
        InformProgressReaders(_progressService.GameState);
        
        _stateMachine.Enter<GameloopState>();

        _loadingScreen.Hide();
    }

    private void InformProgressReaders(GameState gameState)
    {
        for (var i = 0; i < _gameFactory.ProgressReaders.Count; i++)
        {
            _gameFactory.ProgressReaders[i].LoadProgress(gameState);
        }
    }
}