public class Game
{
    public GameStateMachine StateMachine { get; private set; }

    public Game(GameStateMachine gameStateMachine)
    {
        StateMachine = gameStateMachine;
    }
}