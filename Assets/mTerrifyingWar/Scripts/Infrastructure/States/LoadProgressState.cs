using System;

public class LoadProgressState : IState
{
    private readonly GameStateMachine _stateMachine;
    private readonly IStorageService _storageService;
    private readonly IPersistentProgressService _progressService;
    private readonly LoadingScreen _loadingScreen;
    
    public LoadProgressState(GameStateMachine stateMachine, IStorageService storageService, IPersistentProgressService progressService, LoadingScreen loadingScreen)
    {
        _stateMachine = stateMachine;
        _storageService = storageService;
        _progressService = progressService;
        _loadingScreen = loadingScreen;
    }
    
    public void Enter()
    {
        _loadingScreen.Show();
        
        LoadProgressOrInitNew();
        
        _stateMachine.Enter<LoadLevelState, string>(_progressService.GameState.PlayerEntity.PositionOnLevel.Level);
    }
    
    public void Exit() { }
    
    private void LoadProgressOrInitNew()
    {
        _progressService.GameState = _storageService.Load() ?? NewProgress();
    }
    
    private GameState NewProgress()
    {
        GameState gameState = new GameState
        {
            CreationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            PlayerEntity = new PlayerEntity()
        };

        gameState.PlayerEntity.PositionOnLevel.Level = Scenes.City;
        
        gameState.PlayerEntity.HealthEntity.MaxHealth = 100;
        gameState.PlayerEntity.HealthEntity.ResetHealth();
        
        return gameState;
    }
}