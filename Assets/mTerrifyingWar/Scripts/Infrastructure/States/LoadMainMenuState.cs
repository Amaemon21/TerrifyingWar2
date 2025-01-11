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
        _cursorStateService.EnableCursor();
        _sceneLoader.Load(Constans.MainMenu, OnLoaded);
    }

    public void Exit()
    {
        _cursorStateService.DisableCursor();
    }

    private void OnLoaded()
    {
        _loadingScreen.Hide();
    }
}