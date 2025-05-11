using System;

public interface IStorageService
{
    public void Initialize();
    public void Save(GameState gameState, Action<bool> callback = null);
    public void Load(Action<GameState> callback = null);
    public void CreateNewData(Action callback = null);
    public void SetupKey(string key, Action callback = null);
}