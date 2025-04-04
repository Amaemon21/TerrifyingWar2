using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [Space(10)]
    [SerializeField] private EnemyDatabaseConfig _enemyDatabaseConfig;
    
    [Space(10)]
    [SerializeField] private PlayerProvider _playerProvider;
    [SerializeField] private WeaponProvider _weaponProvider;
    [SerializeField] private DisplayProvider _displayProvider;
    
    [Space(10)]
    [SerializeField] private FPSPlayerSettings _playerSettings;
    
    public override void InstallBindings()
    {
        BindServices();
        UIWindowServiceBindings();
        ConfigBindings();
        
        Container.Bind<FPSPlayerSettings>().FromInstance(_playerSettings).AsSingle();
    }
    
    private void BindServices()
    {
        Container.Bind<PlayerProvider>().FromInstance(_playerProvider).AsSingle();
        Container.Bind<DisplayProvider>().FromInstance(_displayProvider).AsSingle();
        Container.Bind<WeaponProvider>().FromInstance(_weaponProvider).AsSingle();
    }
    
    private void UIWindowServiceBindings()
    {
        Container.Bind<UIWindowService>().AsSingle();
    }
    
    private void ConfigBindings()
    {
        Container.Bind<EnemyDatabaseConfig>().FromInstance(_enemyDatabaseConfig).AsSingle();
    }
}