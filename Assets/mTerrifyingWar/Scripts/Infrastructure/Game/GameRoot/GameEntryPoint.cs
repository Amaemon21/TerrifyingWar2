using System.IO;
using UnityEngine;
using Zenject;

public class GameEntryPoint : IInitializable
{
    private readonly DiContainer _container;
    private readonly GameStateMachine _gameStateMachine;
    private readonly JsonProjectSettings _jsonProjectSettings;
    private readonly IStorageService _storageService;
    
    public GameEntryPoint(DiContainer container, GameStateMachine gameStateMachine, JsonProjectSettings jsonProjectSettings, IStorageService storageService)
    {
        _container = container;
        _gameStateMachine = gameStateMachine;
        _jsonProjectSettings = jsonProjectSettings;
        _storageService = storageService;
    }

    public void Initialize()
    {
        _jsonProjectSettings.Initialize();
        _storageService.Initialize();

#if UNITY_EDITOR
        SelectFistSaveFile();
#endif

        InitializeStates();
        
        _gameStateMachine.Enter<BootstrapState>();
    }

    private void InitializeStates()
    {
        BootstrapState bootstrapState = _container.Instantiate<BootstrapState>();
        LoadAuthorizationState loadAuthorizationState = _container.Instantiate<LoadAuthorizationState>();
        LoadMainMenuState loadMainMenuState = _container.Instantiate<LoadMainMenuState>();
        LoadLevelState loadLevelState = _container.Instantiate<LoadLevelState>();
        GameloopState gameloopState = _container.Instantiate<GameloopState>();
        
        _gameStateMachine.AddState(bootstrapState);
        _gameStateMachine.AddState(loadAuthorizationState);
        _gameStateMachine.AddState(loadMainMenuState);
        _gameStateMachine.AddState(loadLevelState);
        _gameStateMachine.AddState(gameloopState);
    }

    private void SelectFistSaveFile()
    {
        string savesDirectoryPath = Application.dataPath + "/SavedData/";
        
        string[] files = Directory.GetFiles(savesDirectoryPath, "*.json");

        if (files.Length != 0)
        {
            string fileName = Path.GetFileNameWithoutExtension(files[0]);

            _storageService.SetupKey(fileName);
        }
    }
}