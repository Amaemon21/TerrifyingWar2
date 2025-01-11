using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private LoadingScreen _loadingScreen;

    public override void InstallBindings()
    {
        BindLoadingScreen();
        BindServices();
        BindGame();
    }

    private void BindGame()
    {
        Container.Bind<GameStateMachine>().AsSingle();
        
        Container.Bind<Game>().AsSingle();
    }

    private void BindLoadingScreen()
    {
        Container.Bind<SceneLoader>().FromNew().AsSingle();
        
        Container.Bind<LoadingScreen>().FromComponentInNewPrefab(_loadingScreen).AsSingle();
    }

    private void BindServices()
    {
        //Container.Bind<IStorageService>().To<StorageService>().FromNew().AsSingle();
        //Container.Bind<IKeysProvider>().To<SaveDataKeysProvider>().FromNew().AsSingle();
        
        Container.Bind<IInputService>().To<InputService>().FromNew().AsSingle();
        
        Container.Bind<CursorStateService>().FromNew().AsSingle();
        
        Container.Bind<IAssetProvider>().To<AssetProvider>().FromNew().AsSingle();

        Container.Bind<IGameFactory>().To<GameFactory>().FromNew().AsSingle();
        
    }
}