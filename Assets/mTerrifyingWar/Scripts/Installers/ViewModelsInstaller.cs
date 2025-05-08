using MVVM;
using Zenject;

public class ViewModelsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<HealthViewModel>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<MoneyViewModel>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ItemInfoViewModel>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<IntertactViewModel>().AsSingle().NonLazy();
    }
}