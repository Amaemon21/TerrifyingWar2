using Zenject;

public class BunkerEnterInteractDoor : InteractObject
{
    [Inject] private readonly GameStateMachine _gameStateMachine;
    
    protected override void OnInteract()
    {
        _gameStateMachine.Enter<LoadLevelState, string>(Scenes.Bunker);
    }
}