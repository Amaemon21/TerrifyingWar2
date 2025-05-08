using UnityEngine;
using Zenject;

public class LoadGameButton : MonoBehaviour
{
    [Inject] private GameStateMachine _gameStateMachine;
    
    public void HandleLoadGameButton()
    {
        //_gameStateMachine.Enter<LoadGameplayState>();
    }
}