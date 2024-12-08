using UnityEngine;

public class GameBootstrapper : MonoBehaviour
{
    public void Init(Game game)
    {
        game.StateMachine.Enter<BootstrapState>();
        DontDestroyOnLoad(this);
    }
}