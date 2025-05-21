using System.IO;
using UnityEngine;
using Zenject;

public class GameEntryPoint : IInitializable
{
    private readonly DiContainer _container;
    private readonly GameStateMachine _gameStateMachine;
    private readonly JsonProjectSettings _jsonProjectSettings;
    
    public GameEntryPoint(DiContainer container, GameStateMachine gameStateMachine, JsonProjectSettings jsonProjectSettings)
    {
        _container = container;
        _gameStateMachine = gameStateMachine;
        _jsonProjectSettings = jsonProjectSettings;
    }

    public void Initialize()
    {
        _jsonProjectSettings.Initialize();
        
        InitializeStates();
        
        _gameStateMachine.Enter<BootstrapState>();
    }

    private void InitializeStates()
    {
        BootstrapState bootstrapState = _container.Instantiate<BootstrapState>();
        LoadAuthorizationState loadAuthorizationState = _container.Instantiate<LoadAuthorizationState>();
        LoadMainMenuState loadMainMenuState = _container.Instantiate<LoadMainMenuState>();
        LoadProgressState loadProgressState = _container.Instantiate<LoadProgressState>();
        LoadLevelState loadLevelState = _container.Instantiate<LoadLevelState>();
        GameloopState gameLoopState = _container.Instantiate<GameloopState>();
        
        _gameStateMachine.AddState(bootstrapState);
        _gameStateMachine.AddState(loadAuthorizationState);
        _gameStateMachine.AddState(loadMainMenuState);
        _gameStateMachine.AddState(loadProgressState);
        _gameStateMachine.AddState(loadLevelState);
        _gameStateMachine.AddState(gameLoopState);
    }
}