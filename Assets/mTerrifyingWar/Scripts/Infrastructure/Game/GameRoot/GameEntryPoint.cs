using Zenject;

public class GameEntryPoint : IInitializable
{
    private GameStateMachine _stateMachine;

    public GameEntryPoint(GameStateMachine gameStateMachine)
    {
        _stateMachine = gameStateMachine;
    }

    public void Initialize()
    {
        _stateMachine.Enter<BootstrapState>();
    }
}