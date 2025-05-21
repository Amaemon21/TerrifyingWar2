using UnityEngine.SceneManagement;
using Zenject;

public class BunkerExitInteractDoor : InteractObject
{
    [Inject] private readonly GameStateMachine _gameStateMachine;
    [Inject] private readonly CityEnterParams _cityEnterParams;
    
    protected override void OnInteract()
    {
        _cityEnterParams.LastScene = SceneManager.GetActiveScene().name;
        
        _gameStateMachine.Enter<LoadLevelState, string>(Scenes.City);
    }
}