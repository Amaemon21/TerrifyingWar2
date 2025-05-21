public interface ISavedProgressReader
{
    public void LoadProgress(GameState gameState);
}

public interface IProgressUpdater : ISavedProgressReader
{
    public void UpdateProgress(GameState gameState);
}