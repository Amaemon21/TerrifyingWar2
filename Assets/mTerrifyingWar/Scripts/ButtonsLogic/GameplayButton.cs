using UnityEngine;
using Zenject;

public class GameplayButton : MonoBehaviour
{
    [Inject] private readonly Game _game;

    public void HandleGameplayLoad()
    {
        _game.StateMachine.Enter<LoadGameplayState>();
    }
}