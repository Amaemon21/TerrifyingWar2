using UnityEngine;

public class LoadAuthorizationState : IState
{ 
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingScreen _loadingScreen;
    private readonly CursorStateService _cursorStateService;
    
    public LoadAuthorizationState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingScreen loadingScreen, CursorStateService cursorStateService)
    {
        _stateMachine = stateMachine;
        _sceneLoader = sceneLoader;
        _loadingScreen = loadingScreen;
        _cursorStateService = cursorStateService;
    }
    
    public void Exit()
    {        
        _loadingScreen.Show();
        
        _sceneLoader.Load(Scenes.Authorization, OnLoaded);
    }

    public void Enter()
    {
    }
    
    private void OnLoaded()
    {
        AuthorizationEntryPoint authorizationEntryPoint = Object.FindFirstObjectByType<AuthorizationEntryPoint>();
        authorizationEntryPoint.Run();
        _cursorStateService.EnableCursor();
        _loadingScreen.Hide();
    }
}