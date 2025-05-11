using System.IO;
using UnityEngine;
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

#if UNITY_EDITOR
        SelectFistSaveFile();
#endif
        
        _gameStateMachine.Enter<BootstrapState>();
    }

    private void SelectFistSaveFile()
    {
        string savesDirectoryPath = Application.dataPath + "/SavedData/";
        
        string[] files = Directory.GetFiles(savesDirectoryPath, "*.json");
        
        string fileName = Path.GetFileNameWithoutExtension(files[0]);

        _storageService.SetupKey(fileName);
    }
}