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
        _loadingScreen.Show(_sceneLoader.Progress);
        _sceneLoader.Load(Constans.Gameplay, OnLoaded);
    }

    public void Exit()
    {

    }

    private void OnLoaded()
    {
        _loadingScreen.Hide();
        _stateMachine.Enter<GameLoopState>();
    }
}