using Object = UnityEngine.Object;

public class LoadLevelState : IPayloadedState<string>
{
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;

    private GameState _gameState;

    public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader)
    {
        _stateMachine = stateMachine;
        _sceneLoader = sceneLoader;
    }

    public void Enter(string sceneName)
    {
        _sceneLoader.Load(sceneName, Load);
    }

    public void Exit() { }
    
    private void Load()
    {
        LoadGameplayEntryPoint();
    }
    
    private void LoadGameplayEntryPoint()
    {
        GameplayEntryPoint gameplayEntryPoint = Object.FindFirstObjectByType<GameplayEntryPoint>();
        gameplayEntryPoint.Run();
    }
}