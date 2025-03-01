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
        BindGameplayEntryPoint();
        BindServices();
        UIWindowServiceBindings();
        ConfigBindings();
    }
    
    private void BindServices()
    {
        Container.BindInterfacesAndSelfTo<GameplayAssetProvider>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameplayFactory>().AsSingle();
        Container.Bind<PlayerProvider>().AsSingle();
        Container.Bind<DisplayProvider>().AsSingle();
    }
    
    private void BindGameplayEntryPoint()
    {
        Container.Bind<PlayerSpawn>().FromInstance(_playerSpawn).AsSingle();
    }
    
    private void UIWindowServiceBindings()
    {
        Container.Bind<UIWindowService>().AsSingle();
    }
    
    private void ConfigBindings()
    {
        Container.Bind<PlayerSettingsConfig>().FromInstance(_playerSettingsConfig).AsSingle();
    }
}