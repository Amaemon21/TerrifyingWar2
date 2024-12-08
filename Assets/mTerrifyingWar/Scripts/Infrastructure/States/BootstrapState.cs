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
        _sceneLoader.Load(Constans.Boot, EnterLoadMainMenu);
    }

    public void Exit()
    {
    }

    private void EnterLoadMainMenu()
    {
        _stateMachine.Enter<LoadMainMenuState, string>(Constans.MainMenu);
    }
}