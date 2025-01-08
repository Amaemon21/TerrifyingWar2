using UnityEngine;
using Zenject;

public class EnemyInstaller : MonoInstaller
{
    [SerializeField] private HeadTransform _headTransform;

    public override void InstallBindings()
    {
        Container.Bind<HeadTransform>().FromInstance(_headTransform).AsSingle();
    }
}