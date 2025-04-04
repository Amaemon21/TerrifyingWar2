using System;
using Zenject;

public class GameEntryPoint : IInitializable
{
    private SceneLoader _sceneLoader;
    private LoadingScreen _loadingScreen;
    private CursorStateService _cursorStateService;
    
    public GameEntryPoint(SceneLoader sceneLoader, LoadingScreen loadingScreen, CursorStateService cursorStateService)
    {
        _sceneLoader = sceneLoader;
        _loadingScreen = loadingScreen;
        _cursorStateService = cursorStateService;
    }

    public void Initialize()
    {
#if UNITY_EDITOR
        string sceneName = _sceneLoader.GetSceneName();

        if (sceneName == Scenes.Gameplay)
        {
            return;
        }

        if (sceneName == Scenes.MainMenu)
        {
            LoadBoot(LoadMainMenu);
            return;
        }
        
        if (sceneName == Scenes.Authorization)
        {
            LoadBoot(LoadAuthorization);
            return;
        }

        if (sceneName != Scenes.Boot)
        {
            return;
        }
#endif
        LoadBoot(LoadAuthorization);
    }
    
    private void LoadBoot(Action callback)
    {
        _cursorStateService.DisableCursor();
        _loadingScreen.Show();
        _sceneLoader.Load(Scenes.Boot, callback);
    }
    
    private void LoadAuthorization()
    {
        _loadingScreen.Show();
        _sceneLoader.Load(Scenes.Authorization, OnMainMenuLoaded);
    }
    
    public void LoadMainMenu()
    {
        _loadingScreen.Show();
        _sceneLoader.Load(Scenes.MainMenu, OnMainMenuLoaded);
    }
    
    public void LoadGameplay()
    {
        _loadingScreen.Show();
        _sceneLoader.Load(Scenes.Gameplay, OnGameplayLoaded);
    }

    private void OnGameplayLoaded()
    {
        _cursorStateService.DisableCursor();
        _loadingScreen.Hide();
    }
    
    private void OnMainMenuLoaded()
    {
        _cursorStateService.EnableCursor();
        _loadingScreen.Hide();
    }
}