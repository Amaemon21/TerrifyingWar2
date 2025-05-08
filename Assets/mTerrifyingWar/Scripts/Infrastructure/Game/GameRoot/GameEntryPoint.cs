using Zenject;

public class GameEntryPoint : IInitializable
{
    private readonly GameStateMachine _gameStateMachine;
    private readonly JsonProjectSettings _jsonProjectSettings;
    private readonly IStorageService _storageService;
    
    public GameEntryPoint(GameStateMachine gameStateMachine, JsonProjectSettings jsonProjectSettings, IStorageService storageService)
    {
        _gameStateMachine = gameStateMachine;
        _jsonProjectSettings = jsonProjectSettings;
        _storageService = storageService;
    }

    public void Initialize()
    {
        _jsonProjectSettings.Initialize();
        _storageService.Initialize();
        
        _gameStateMachine.Enter<BootstrapState>();
    }
}