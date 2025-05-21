using Zenject;

public class BunkerEnterInteractDoor : InteractObject
{
    [Inject] private readonly IStorageService _storageService;
    [Inject] private readonly GameStateMachine _gameStateMachine;
    
    protected override void OnInteract()
    {
        _storageService.Save();
        
        _gameStateMachine.Enter<LoadLevelState, string>(Scenes.Bunker);
    }
}