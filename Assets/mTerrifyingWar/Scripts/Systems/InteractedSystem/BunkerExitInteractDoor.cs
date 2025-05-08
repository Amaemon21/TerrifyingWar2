using Zenject;

public class BunkerExitInteractDoor : InteractObject
{
    [Inject] private readonly GameStateMachine _gameStateMachine;
    
    protected override void OnInteract()
    {
        _gameStateMachine.Enter<LoadGameplayState, string>(Scenes.Gameplay);
    }
}