using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private LoadingScreen _loadingScreen;
    
    [SerializeField] private PlayerSettingsConfig _playerSettingsConfig;
    
    public override void InstallBindings()
    {
        BindLoadingScreen();
        BindServices();
        BindGameEntryPoint();
        
        Container.Bind<BackendManager>().AsSingle();
        
        Container.Bind<PlayerSettingsConfig>().FromInstance(_playerSettingsConfig).AsSingle();
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