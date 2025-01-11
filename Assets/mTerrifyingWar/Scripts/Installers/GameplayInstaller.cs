using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private PlayerController _playerController;

    public override void InstallBindings()
    {
        PlayerBindings();
        UIWindowServiceBindings();
    }
    
    private void PlayerBindings()
    {
        Container.Bind<PlayerController>().FromInstance(_playerController).AsSingle();
    }
    
    private void UIWindowServiceBindings()
    {
        Container.Bind<UIWindowService>().FromNew().AsSingle();
    }
}