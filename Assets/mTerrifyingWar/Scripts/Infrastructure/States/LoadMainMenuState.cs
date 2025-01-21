using UnityEngine;

public class LoadMainMenuState : IPayloadedState<IExitableState>
{
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingScreen _loadingScreen;

    public LoadMainMenuState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingScreen loadingScreen)
    {
        _stateMachine = stateMachine;
        _sceneLoader = sceneLoader;
        _loadingScreen = loadingScreen;
    }

    public void Enter(IExitableState payload)
    {
        if (payload is BootstrapState bootstrapState)
            _loadingScreen.Hide();
        else
            _loadingScreen.Show();
        
        _sceneLoader.Load(Scenes.MainMenu, OnLoaded);
    }

    public void Exit()
    {
        
    }

    private void OnLoaded()
    {
        _loadingScreen.Hide();
    }
}