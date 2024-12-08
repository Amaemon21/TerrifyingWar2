public class GameLoopState : IState
{
    private readonly GameStateMachine _stateMachine;
    private readonly CursorStateService _cursorStateService;
    
    public GameLoopState(GameStateMachine stateMachine, CursorStateService cursorStateService)
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
    } 
}