public class GameloopState : IState
{
    private readonly GameStateMachine _stateMachine;
    private readonly IGameFactory _gameFactory;
    private readonly CursorStateService _cursorStateService;
    
    public GameloopState(GameStateMachine stateMachine, IGameFactory gameFactory, CursorStateService cursorStateService)
    {
        _stateMachine = stateMachine;
        _gameFactory = gameFactory;
        _cursorStateService = cursorStateService;
    }

    public void Enter()
    {
        _cursorStateService.DisableCursor();
    }

    public void Exit()
    {
        _gameFactory.CleanUp();
        _stateMachine.RemoveState<LevelState>();
    }
}