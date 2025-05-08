using System;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class StorageService : IStorageService
{
    private string _saveDirectory;
    private string _key;
    
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
        SaveData saveData = new SaveData
        {
            CreationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };
        
        Save(saveData, success => callback?.Invoke());
    }

    public void Save(SaveData _saveData, Action<bool> callback = null)
    {
        if (string.IsNullOrEmpty(_key))
            return;
        
        SaveAsync(_saveData).ContinueWith(success => callback?.Invoke(success));
    }
    
    public void Load(Action<SaveData> callback = null)
    {
        if (string.IsNullOrEmpty(_key))
            return;

        LoadAsync().ContinueWith(data => callback?.Invoke(data));
    }

    private async UniTask<bool> SaveAsync(SaveData _saveData)
    {
        try
        {
            string path = BuildPath(_key);
            string json = JsonConvert.SerializeObject(_saveData, Formatting.Indented);
            await File.WriteAllTextAsync(path, json);
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to save GameState: {ex.Message}");
            return false;
        }
    }
    
    private async UniTask<SaveData> LoadAsync()
    {
        try
        {
            string path = BuildPath(_key);
            
            if (!File.Exists(path))
            {
                Debug.LogWarning($"No saved GameState found at {path}");
                return null;
            }
            
            string json = await File.ReadAllTextAsync(path);
            SaveData data = JsonConvert.DeserializeObject<SaveData>(json);
            return data;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load GameState: {ex.Message}");
            return null;
        }
    }

    private string BuildPath(string key)
    {
        return Path.Combine(_saveDirectory, $"{key}.json");
    }
}