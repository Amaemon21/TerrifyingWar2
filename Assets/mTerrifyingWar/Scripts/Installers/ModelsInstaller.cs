using Zenject;

public class ModelsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<PlayerHealth>().AsSingle().NonLazy();
        Container.Bind<MoneyModel>().AsSingle().NonLazy();
        Container.Bind<ExperienceModel>().AsSingle().NonLazy();
        Container.Bind<ItemInfo>().AsSingle().NonLazy();
        Container.Bind<InteractModel>().AsSingle().NonLazy();
    }
}