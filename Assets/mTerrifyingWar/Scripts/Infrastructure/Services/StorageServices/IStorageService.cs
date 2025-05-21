using System;

public interface IStorageService : IService
{
    public void Save(Action callback = null);
    public GameState Load();
}