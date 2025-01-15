using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private LoadingScreen _loadingScreen;
    
    public override void InstallBindings()
    {
        BindGameStateMachine();
        BindLoadingScreen();
        BindServices();
        BindGameEntryPoint();
    }
    
    private void BindGameStateMachine()
    {
        Container.Bind<GameStateMachine>().FromNew().AsSingle();
    }
    
    private void BindLoadingScreen()
    {
        Container.Bind<SceneLoader>().FromNew().AsSingle();
        
        Container.Bind<LoadingScreen>().FromComponentInNewPrefab(_loadingScreen).AsSingle();
    }

    private void BindServices()
    {
        Container.Bind<IStorageService>().To<StorageService>().FromNew().AsSingle();
        Container.Bind<IKeysProvider>().To<SaveDataKeysProvider>().FromNew().AsSingle();
        
        Container.Bind<IInputService>().To<InputService>().FromNew().AsSingle();
        
        Container.Bind<CursorStateService>().FromNew().AsSingle();
    }

    private void BindGameEntryPoint()
    {
        Container.BindInterfacesAndSelfTo<GameEntryPoint>().AsSingle().NonLazy();
    }
}