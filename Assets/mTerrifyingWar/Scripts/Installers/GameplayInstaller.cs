using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private PlayerSpawnPosition _playerSpawnPosition;

    public override void InstallBindings()
    {
        BindUI();
        BindServices();
        BindGameplaySystems();
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
        Container.Bind<PlayerSpawnPosition>().FromInstance(_playerSpawnPosition).AsSingle();
        
        Container.Bind<QuestTracker>().AsSingle().NonLazy();
        Container.Bind<QuestEvents>().AsSingle().NonLazy();
    }
}