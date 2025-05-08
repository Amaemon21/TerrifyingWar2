using UnityEngine;

public class LoadGameplayState : IPayloadedState<string>
{
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingScreen _loadingScreen;

    private SaveData _saveData;
    
    public LoadGameplayState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingScreen loadingScreen)
    {
        _stateMachine = stateMachine;
        _sceneLoader = sceneLoader;
        _loadingScreen = loadingScreen;
    }

    public void Enter(string sceneName)
    {
        _loadingScreen.Show();

        _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit()
    {
    }
    
    private void OnLoaded()
    {
        var gameplayEntryPoint = Object.FindFirstObjectByType<GameplayEntryPoint>();
        gameplayEntryPoint.Run();
    }
}