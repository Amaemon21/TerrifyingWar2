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
        Run();
    }

    public void Exit()
    {
    }

    private void Run()
    {
#if UNITY_EDITOR
        HandleEditorScenes();
#else
        LoadBootAndMainMenu();
#endif
    }
    
    private void HandleEditorScenes()
    {
        switch (_sceneLoader.GetSceneName())
        {
            case Constans.Gameplay:
                LoadBootAndGameplay();
                return;
            case Constans.MainMenu:
                LoadBootAndMainMenu();
                return;
            case Constans.Boot:
                LoadBootAndMainMenu();
                return;
        }
    }

    private void LoadBootAndGameplay()
    {
        _sceneLoader.Load(Constans.Boot, () =>
        {
            _sceneLoader.Load(Constans.Gameplay, () =>
            {
                _stateMachine.Enter<LoadGameplayState>();
            });
        });
    }

    private void LoadBootAndMainMenu()
    {
        _sceneLoader.Load(Constans.Boot, () =>
        {
            _sceneLoader.Load(Constans.MainMenu, () =>
            {
                _stateMachine.Enter<LoadMainMenuState>();
            });
        });
    }
}