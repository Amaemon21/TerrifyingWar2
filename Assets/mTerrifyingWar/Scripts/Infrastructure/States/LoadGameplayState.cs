using UnityEngine;

public class LoadGameplayState : IState
{
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingScreen _loadingScreen;

    public LoadGameplayState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingScreen loadingScreen)
    {
        _stateMachine = stateMachine;
        _sceneLoader = sceneLoader;
        _loadingScreen = loadingScreen;
    }

    public void Enter()
    {
        _loadingScreen.Show();
        _sceneLoader.Load(Scenes.Gameplay, OnLoaded);
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