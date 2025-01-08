using mTerrifyingWar.Scripts.Game.Gameplay.Root;
using mTerrifyingWar.Scripts.Game.GameRoot;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEntryPoint
{
    private static GameEntryPoint _instance;
    
    private SceneLoader _sceneLoader;
    private UIRootView _uiRoot;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void AutostartGame()
    {
        //_instance = new GameEntryPoint();
        //_instance.RunGame();
    }

    private GameEntryPoint()
    {
        _sceneLoader = new SceneLoader();
        
        UIRootView prefabUIRoot = Resources.Load<UIRootView>("UIRoot");
        _uiRoot = Object.Instantiate(prefabUIRoot);
        _uiRoot.Setup(_sceneLoader);
        Object.DontDestroyOnLoad(_uiRoot.gameObject);
        
        //
    }
    
    private void RunGame()
    {
#if UNITY_EDITOR
        var sceneName  = SceneManager.GetActiveScene().name;

        if (sceneName == Constans.Gameplay)
        {
            LoadAndStartGameplay();
            return;
        }

        if (sceneName != Constans.Boot)
        {
            return;
        }
#endif

        LoadAndStartGameplay();
    }

    private void LoadAndStartGameplay()
    {
        _uiRoot.LoadingScreen.Show();
        _sceneLoader.Load(Constans.Boot, OnLoadedBoot);
    }

    private void OnLoadedBoot()
    {
        _sceneLoader.Load(Constans.Gameplay, OnLoadedGameplay);
    }
    
    private void OnLoadedGameplay()
    {
        _uiRoot.LoadingScreen.Hide();
        
        GameplayEntryPoint sceneEntryPoint = Object.FindFirstObjectByType<GameplayEntryPoint>();
        sceneEntryPoint.Run(_uiRoot);
    }
}
