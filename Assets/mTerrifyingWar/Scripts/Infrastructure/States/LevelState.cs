using System.Collections;
using UnityEngine;

public class LevelState : IState
{
    private readonly GameStateMachine _stateMachine;
    private readonly IGameFactory _gameFactory;
    private readonly IGameplayFactory _gameplayFactory;
    private readonly LoadingScreen _loadingScreen;
    private readonly Coroutines _coroutines;
    private readonly IPersistentProgressService _persistentProgressService;

    public LevelState(GameStateMachine stateMachine, IGameFactory gameFactory, IGameplayFactory gameplayFactory, LoadingScreen loadingScreen,
        Coroutines coroutine, IPersistentProgressService persistentProgressService)
    {
        _stateMachine = stateMachine;
        _gameFactory = gameFactory;
        _gameplayFactory = gameplayFactory;
        _loadingScreen = loadingScreen;
        _coroutines = coroutine;
        _persistentProgressService = persistentProgressService;
    }

    public void Enter()
    {
        _coroutines.StartCoroutine(SetupWorld());
    }

    public void Exit()
    {
    }

    private IEnumerator SetupWorld()
    {
        var playerSpawnPosition = Object.FindFirstObjectByType<PlayerSpawnPosition>();

        _gameplayFactory.CreatePlayer(playerSpawnPosition.transform);
        _gameplayFactory.CreateHud();

        yield return null;

        InformProgressReaders();
        _stateMachine.Enter<GameloopState>();
        _loadingScreen.Hide();
    }

    private void InformProgressReaders()
    {
        foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
        {
            progressReader.LoadProgress(_persistentProgressService.GameState);
        }
    }
}