using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private LoadingScreen _loadingScreen;
    [SerializeField] private Coroutines _coroutines;
    [SerializeField] private PlayerSettingsConfig _playerSettingsConfig;

    public override void InstallBindings()
    {
        BindLoadingScreen();
        BindPlayerSettings();
        BindGameplaySystems();
        BindGameStateMachine();
        BindGameEntryPoint();

        BindParams();
    }

    private void BindLoadingScreen()
    {
        Container.Bind<Coroutines>().FromComponentInNewPrefab(_coroutines).AsSingle();
        Container.Bind<SceneLoader>().AsSingle();
        Container.Bind<LoadingScreen>().FromComponentInNewPrefab(_loadingScreen).AsSingle();
    }

    private void BindPlayerSettings()
    {
        Container.Bind<PlayerSettingsConfig>().FromInstance(_playerSettingsConfig).AsSingle();
    }
    
    private void BindGameplaySystems()
    {
        Container.BindInterfacesAndSelfTo<GameFactory>().AsSingle();
        
        Container.Bind<JsonProjectSettings>().AsSingle();
        Container.BindInterfacesAndSelfTo<PersistentProgressService>().AsSingle();
        Container.BindInterfacesAndSelfTo<StorageService>().AsSingle();
        
        Container.Bind<BackendManager>().AsSingle();
        Container.Bind<CursorStateService>().AsSingle();
    }

    private void BindGameStateMachine()
    {
        Container.Bind<GameStateMachine>().AsSingle();
    }

    private void BindGameEntryPoint()
    {
        Container.BindInterfacesAndSelfTo<GameEntryPoint>().AsSingle().NonLazy();
    }

    private void BindParams()
    {
        Container.Bind<CityEnterParams>().AsSingle();
    }
}