public interface ISavedProgressReader
{
    public void LoadProgress(GameState gameState);
}

public interface ISavedProgress : ISavedProgressReader
{
    public void UpdateProgress(GameState gameState);
}
