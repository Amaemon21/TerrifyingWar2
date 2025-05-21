using System;
using System.IO;
using UnityEngine;

public class StorageService : IStorageService
{
    private readonly IGameFactory _gameFactory;
    private readonly IPersistentProgressService _persistentProgressService;
    
    private readonly string _dataKey = "ProgressKey";
    
    private string _saveDirectory;

    public StorageService(IGameFactory gameFactory, IPersistentProgressService persistentProgressService)
    {
        _gameFactory = gameFactory;
        _persistentProgressService = persistentProgressService;
    }

    public void Save(Action callback = null)
    {
        if (_gameFactory.ProgressWriters.Count > 0)
        {
            foreach (var progressWriter in _gameFactory.ProgressWriters)
                progressWriter.UpdateProgress(_persistentProgressService.GameState);
        }
        
        //PlayerPrefs.SetString(_dataKey, _persistentProgressService.GameState.ToJson());
        
        string path = BuildPath(_dataKey);
        string json = _persistentProgressService.GameState.ToJson();
        File.WriteAllText(path, json);
    }
    
    public GameState Load()
    {
        try
        {
            string path = BuildPath(_dataKey);
            
            if (!File.Exists(path))
            {
                return null;
            }
            
            string json = File.ReadAllText(path);
            GameState data = json.ToDeserialized<GameState>();
            return data;
        }
        catch
        {
            return null;
        }
    }
    
    private string BuildPath(string key)
    {
#if UNITY_EDITOR
        _saveDirectory = Path.Combine(Application.dataPath, "SavedData");
#else
        _saveDirectory = Application.persistentDataPath;
#endif
        
        Directory.CreateDirectory(_saveDirectory);
        
        return Path.Combine(_saveDirectory, $"{key}.json");
    }
}
