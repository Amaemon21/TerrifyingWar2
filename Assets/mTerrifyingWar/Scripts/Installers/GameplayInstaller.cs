using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [Space(10)]
    [SerializeField] private SpawnPlayerPoint _spawnPlayerPoint;
    
    public override void InstallBindings()
    {
        BindServices();

        BindGameplayEntryPoint();
        
        Container.Bind<DisplayProvider>().FromNew().AsSingle();
        
        UIWindowServiceBindings();
    }
    
    private void BindServices()
    {
        Container.Bind<IAssetProvider>().To<GameplayAssetProvider>().AsSingle();
        Container.Bind<PlayerProvider>().AsSingle();
        Container.Bind<IGameplayFactory>().To<GameplayFactory>().AsSingle();
    }
    
    private void BindGameplayEntryPoint()
    {
        Container.Bind<SpawnPlayerPoint>().FromInstance(_spawnPlayerPoint).AsSingle();
    }
    
    private void UIWindowServiceBindings()
    {
        Container.Bind<UIWindowService>().FromNew().AsSingle();
    }
}