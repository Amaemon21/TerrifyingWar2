using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class StorageService : IStorageService
{
    private readonly IGameFactory _gameFactory;
    private readonly IPersistentProgressService _persistentProgressService;
    
    private string _saveDirectory;
    private string _key;

    public StorageService(IGameFactory gameFactory, IPersistentProgressService persistentProgressService)
    {
        _gameFactory = gameFactory;
        _persistentProgressService = persistentProgressService;
    }
    
    public void Initialize()
    {
#if UNITY_EDITOR
        _saveDirectory = Path.Combine(Application.dataPath, "SavedData");
#else
        _saveDirectory = Application.persistentDataPath;
#endif
        Directory.CreateDirectory(_saveDirectory);
    }
    
    public void SetupKey(string key, Action callback = null)
    {
        _key = key;
        callback?.Invoke();
    }
    
    public void CreateNewData(Action callback = null)
    {
        GameState gameState = new GameState
        {
            CreationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        _persistentProgressService.GameState = gameState;
        
        Save();
        
        callback?.Invoke();
    }

    public void Save()
    {
        if (_gameFactory.ProgressWriters == null)
            return;
        
        foreach (var progressWriter in _gameFactory.ProgressWriters)
            progressWriter.UpdateProgress(_persistentProgressService.GameState);

        PlayerPrefs.SetString(_key, _persistentProgressService.GameState.ToJson());
        
        /*
        if (string.IsNullOrEmpty(_key))
            return;
        
        try
        {
            string path = BuildPath(_key);
            string json = JsonConvert.SerializeObject(gameState, Formatting.Indented);
            File.WriteAllText(path, json);
            callback?.Invoke(true);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to save GameState: {ex.Message}");
        }
        */
    }
    
    public void Load(Action<GameState> callback)
    { 
        var gameState = PlayerPrefs.GetString(_key)?.ToDeserialized<GameState>();
        callback?.Invoke(gameState);
        
        /*
        if (string.IsNullOrEmpty(_key))
            return;

        try
        {
            string path = BuildPath(_key);
            
            if (!File.Exists(path))
            {
                Debug.LogWarning($"No saved GameState found at {path}");
            }
            
            string json = File.ReadAllText(path);
            GameState data = JsonConvert.DeserializeObject<GameState>(json);
            callback?.Invoke(data);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load GameState: {ex.Message}");
        }
        */
    }

    private string BuildPath(string key)
    {
        return Path.Combine(_saveDirectory, $"{key}.json");
    }
}