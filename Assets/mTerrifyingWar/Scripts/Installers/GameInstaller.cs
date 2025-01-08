using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private LoadingScreen _loadingScreen;

    public override void InstallBindings()
    {
        BindLoadingScreen();
        BindServices();
        BindGameStateMachine();
        BindGame();
    }

    private void BindGameStateMachine()
    {
        //Container.Bind<GameStateMachine>().FromNew().AsSingle();
    }

    private void BindLoadingScreen()
    {
        //Container.Bind<SceneLoader>().FromNew().AsSingle();
        
        //LoadingScreen loadingScreen = Container.InstantiatePrefabForComponent<LoadingScreen>(_loadingScreen, transform);
        //Container.Bind<LoadingScreen>().FromInstance(loadingScreen).AsSingle();
    }

    private void BindServices()
    {
        //Container.Bind<IStorageService>().To<StorageService>().FromNew().AsSingle();
        //Container.Bind<IKeysProvider>().To<SaveDataKeysProvider>().FromNew().AsSingle();
        
        Container.Bind<CursorStateService>().FromNew().AsSingle();
        
        //Container.Bind<IAssetProvider>().To<AssetProvider>().FromNew().AsSingle();

        //Container.Bind<IGameFactory>().To<GameFactory>().FromNew().AsSingle();

        Container.Bind<EventBus>().FromNew().AsSingle();
    }
    
    private void BindGame()
    {
        //Container.Bind<Game>().FromNew().AsSingle();
    }
}