using System;

public interface IStorageService : IService
{
    public void Initialize();
    public void Save();
    public void Load(Action<GameState> callback);
    public void CreateNewData(Action callback = null);
    public void SetupKey(string key, Action callback = null);
}