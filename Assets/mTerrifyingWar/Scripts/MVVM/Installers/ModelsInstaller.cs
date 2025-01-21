using Zenject;

public class ModelsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlayerHealth>().AsSingle();
    }
}