using UnityEngine;
using Zenject;

public class GameBootstrapper : MonoBehaviour
{
    [Inject] private Game _game;

    private void Awake()
    {
        _game.Run();
    }
}