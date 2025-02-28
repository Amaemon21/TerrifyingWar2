using Zenject;

public class AuthorizationInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<UIWindowService>().AsSingle();
    }
}
