public class LoadMainMenuState : IState
{
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingScreen _loadingScreen;
    private readonly CursorStateService _cursorStateService;

    public LoadMainMenuState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingScreen loadingScreen, CursorStateService cursorStateService)
    {
        _stateMachine = stateMachine;
        _sceneLoader = sceneLoader;
        _loadingScreen = loadingScreen;
        _cursorStateService = cursorStateService;
    }

    public void Enter()
    {
        _loadingScreen.Show();
        
        _sceneLoader.Load(Scenes.MainMenu, OnLoaded);
    }

    public void Exit()
    {
    }

    private void OnLoaded()
    {
        _cursorStateService.EnableCursor();
        _loadingScreen.Hide();
    }
}