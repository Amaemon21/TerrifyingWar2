using System;

public interface IStorageService
{
    public void Initialize();
    public void Save(SaveData _saveData, Action<bool> callback = null);
    public void Load(Action<SaveData> callback = null);
    public void CreateNewData(Action callback = null);
    public void SetupKey(string key, Action callback = null);
}