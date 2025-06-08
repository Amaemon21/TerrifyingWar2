using UnityEngine.SceneManagement;

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
        LoadBootAndAuthorization();
#endif
    }
    
    private void HandleEditorScenes()
    {
        var sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case Scenes.City:
                LoadBootAndGameplay();
                return;
            case Scenes.MainMenu:
                LoadBootAndMainMenu();
                return;
            case Scenes.Authorization:
                LoadBootAndAuthorization();
                return;
            case Scenes.Boot:
                LoadBootAndAuthorization();
                return;
        }
    }
    
    private void LoadBootAndGameplay()
    {
        _sceneLoader.Load(Scenes.Boot, () =>
        {
            _stateMachine.Enter<LoadProgressState>();
        });
    }

    private void LoadBootAndMainMenu()
    {
        _sceneLoader.Load(Scenes.Boot, () =>
        {
            _sceneLoader.Load(Scenes.MainMenu, () =>
            {
                _stateMachine.Enter<LoadMainMenuState>();
            });
        });
    }
    
    private void LoadBootAndAuthorization()
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