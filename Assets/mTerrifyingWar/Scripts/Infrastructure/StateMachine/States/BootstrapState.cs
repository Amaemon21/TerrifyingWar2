public class BootstrapState : IState
{
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;

    public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader)
    {
        _stateMachine = stateMachine;
        _sceneLoader = sceneLoader;
    }

    public void Enter()
    {
#if UNITY_EDITOR
        string sceneName = _sceneLoader.GetSceneName();

        if (sceneName == Scenes.Gameplay)
        {
            LoadAndStartGameplay();
            return;
        }

        if (sceneName == Scenes.MainMenu)
        {
            LoadAndStartMainMenu();
            return;
        }
        
        if (sceneName == Scenes.Authorization)
        {
            LoadAndStartAuthorization();
            return;
        }

        if (sceneName != Scenes.Boot)
        {
            return;
        }
#endif
        LoadAndStartAuthorization();
    }

    public void Exit() { }
    
    private void LoadAndStartGameplay()
    {
        _sceneLoader.Load(Scenes.Boot, () =>
        {
            _sceneLoader.Load(Scenes.Gameplay, () =>
            {
                _stateMachine.Enter<LoadGameplayState>();
            });
        });
    }

    private void LoadAndStartMainMenu()
    {
        _sceneLoader.Load(Scenes.Boot, () =>
        {
            _sceneLoader.Load(Scenes.MainMenu, () =>
            {
                _stateMachine.Enter<LoadMainMenuState, IExitableState>(_stateMachine.GetActiveState());
            });
        });
    }
    
    private void LoadAndStartAuthorization()
    {
        _sceneLoader.Load(Scenes.Boot, () =>
        {
            _sceneLoader.Load(Scenes.Authorization, () =>
            {
                _stateMachine.Enter<LoadAuthorizationState>();
            });
        });
    }
}