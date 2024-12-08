using System;
using System.IO;
using UnityEngine;

public class StorageService : IStorageService
{
    private bool _isInProgressNow;

    public void Save(string key, object data, Action<bool> callback = null)
    {
        if (!_isInProgressNow)
        {
            _isInProgressNow = true;

            try
            {
                SaveAsync(key, data);
                callback?.Invoke(true);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save data to {key}: {ex}");
                callback?.Invoke(false);
            }
            finally
            {
                _isInProgressNow = false;
            }
        }
        else
        {
            callback?.Invoke(false);
        }
    }

    public void Load<T>(string key, Action<T> callback = null) where T : new()
    {
        string path = BuildPath(key);

        try
        {
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var data = JsonUtility.FromJson<T>(json);
                Debug.Log("Load data - " + key);
                callback?.Invoke(data);
            }
            else
            {
                CreateNewData(key, callback);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load data: {key}");
        }
    }

    private async void SaveAsync(string key, object data)
    {
        string path = BuildPath(key);
        string json = JsonUtility.ToJson(data, true);

        try
        { 
            using (var fileStream = new StreamWriter(path))
            {
                await fileStream.WriteAsync(json);
                Debug.Log("Saved file");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error saving data: {key}");
            throw;
        }
    }
    
    private void CreateNewData<T>(string key, Action<T> callback) where T : new()
    {
        Debug.Log("Create new file data - " + key);

        var newData = new T();

        Save(key, newData);

        callback?.Invoke(newData);
    }

    private string BuildPath(string key)
    {
#if UNITY_EDITOR
        return Application.dataPath + $"/SavedData/{key}.json";
#else
        return Application.persistentDataPath + $"/{key}.json";
#endif
    }
}