using UnityEngine;

public class LoadLevelState : IPayloadedState<string>
{
    private readonly GameStateMachine _stateMachine;
    private readonly IStorageService _storageService;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingScreen _loadingScreen;
    private readonly IPersistentProgressService _persistentProgress;

    private GameState _gameState;

    public LoadLevelState(GameStateMachine stateMachine, IStorageService storageService, SceneLoader sceneLoader,
        LoadingScreen loadingScreen, IPersistentProgressService persistentProgress)
    {
        _stateMachine = stateMachine;
        _storageService = storageService;
        _sceneLoader = sceneLoader;
        _loadingScreen = loadingScreen;
        _persistentProgress = persistentProgress;
    }

    public void Enter(string sceneName)
    {
        _loadingScreen.Show();
        _storageService.Load(LoadData);

        _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit()
    {
    }
    
    private void OnLoaded()
    {
        var gameplayEntryPoint = Object.FindFirstObjectByType<GameplayEntryPoint>();
        gameplayEntryPoint.Run();
    }
    
    private void LoadData(GameState gameState)
    {
        return;
        
        _gameState = gameState;
        _persistentProgress.GameState = gameState;
        
        bool isPlayer = false;
        
        foreach (var entity in gameState.Entities)
        {
            if (entity is PlayerEntity playerEntity)
            {
                isPlayer = true;
            }
        }
        
        if (!isPlayer)
        {
            PlayerEntity playerEntity = new PlayerEntity();
            
            gameState.Entities.Add(playerEntity);
            
            _storageService.Save();
        }
    }
}