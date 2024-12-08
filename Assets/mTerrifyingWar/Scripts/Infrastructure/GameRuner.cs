using UnityEngine;
using Zenject;

public class GameRuner : MonoBehaviour
{
    [Inject] private readonly Game _game;
    
    [SerializeField] private GameBootstrapper _gameBootstrapper;

    private void Awake()
    {
        GameBootstrapper bootstraper = FindFirstObjectByType<GameBootstrapper>();

        if (bootstraper == null)
        {
            GameBootstrapper boot = Instantiate(_gameBootstrapper);
            boot.Init(_game);
        }
    }
}