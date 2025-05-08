public class GameloopState : IState
{
    private readonly GameStateMachine _stateMachine;
    private readonly CursorStateService _cursorStateService;
    
    public GameloopState(GameStateMachine stateMachine, CursorStateService cursorStateService)
    {
        _stateMachine = stateMachine;
        _cursorStateService = cursorStateService;
    }

    public void Enter()
    {
        _cursorStateService.DisableCursor();
    }

    public void Exit()
    {
        _stateMachine.RemoveState<GameplayState>();
    }
}