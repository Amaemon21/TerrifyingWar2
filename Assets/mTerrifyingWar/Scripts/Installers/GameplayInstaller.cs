using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [Space(10)]
    [SerializeField] private PlayerSpawn _playerSpawn;
    
    [Space(10)]
    [SerializeField] private PlayerSettingsConfig _playerSettingsConfig;
    
    public override void InstallBindings()
    {
        BindServices();

        BindGameplayEntryPoint();
        
        UIWindowServiceBindings();
        
        ConfigBindings();
    }
    
    private void BindServices()
    {
        Container.Bind<IAssetProvider>().To<GameplayAssetProvider>().AsSingle();
        Container.Bind<PlayerProvider>().AsSingle();
        Container.Bind<IGameplayFactory>().To<GameplayFactory>().AsSingle();
        Container.Bind<DisplayProvider>().FromNew().AsSingle();
    }
    
    private void BindGameplayEntryPoint()
    {
        Container.Bind<PlayerSpawn>().FromInstance(_playerSpawn).AsSingle();
    }
    
    private void UIWindowServiceBindings()
    {
        Container.Bind<UIWindowService>().FromNew().AsSingle();
    }
    
    private void ConfigBindings()
    {
        Container.Bind<PlayerSettingsConfig>().FromInstance(_playerSettingsConfig).AsSingle();
    }
}