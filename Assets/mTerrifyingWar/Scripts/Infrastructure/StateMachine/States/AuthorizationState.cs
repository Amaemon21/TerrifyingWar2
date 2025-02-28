public class AuthorizationState : IState
{
    private readonly GameStateMachine _stateMachine;
    
    public AuthorizationState(GameStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }
    
    public void Enter()
    {

    }

    public void Exit()
    {
    }
}