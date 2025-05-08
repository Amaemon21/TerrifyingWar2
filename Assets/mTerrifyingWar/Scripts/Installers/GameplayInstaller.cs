using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [Space(10)]
    [SerializeField] private EnemyDatabaseConfig _enemyDatabaseConfig;

    public override void InstallBindings()
    {
        BindConfig();
        BindUI();
        BindServices();
        BindGameplaySystems();
    }

    private void BindConfig()
    {
        Container.Bind<EnemyDatabaseConfig>().FromInstance(_enemyDatabaseConfig).AsSingle();
    }

    private void BindUI()
    {
        Container.Bind<UIWindowService>().AsSingle();
    }

    private void BindServices()
    {
        Container.Bind<PlayerProvider>().AsSingle();
        Container.Bind<DisplayProvider>().AsSingle();
        Container.Bind<IAssetsProvider>().To<AssetsProvider>().AsSingle();
        Container.Bind<IGameplayFactory>().To<GameplayFactory>().FromNew().AsSingle();
        Container.Bind<IInputService>().To<InputService>().FromNew().AsSingle();
    }
    
    private void BindGameplaySystems()
    {
        Container.Bind<QuestTracker>().AsSingle().NonLazy();
        Container.Bind<QuestEvents>().AsSingle().NonLazy();
    }
}
