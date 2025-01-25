using Zenject;

public class ModelsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<PlayerHealth>().AsSingle().NonLazy();
        Container.Bind<ItemInfo>().AsSingle().NonLazy();
        Container.Bind<InteractUIModel>().AsSingle().NonLazy();
    }
}