using UnityEngine;
using Zenject;

public class MainMenuLoadButton : MonoBehaviour
{
    [Inject] private readonly Game _game;
    
    public void HandleMainMenuLoad()
    {
        _game.StateMachine.Enter<LoadMainMenuState>();
    }
}