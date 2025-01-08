using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private UIManager _uiManager;

    public override void InstallBindings()
    {
        PlayerBindings();
        UIManagerBindings();
    }
    
    private void PlayerBindings()
    {
        Container.Bind<PlayerController>().FromInstance(_playerController).AsSingle();
        Container.Bind<IInputService>().To<InputService>().FromNew().AsSingle();
    }
    
    private void UIManagerBindings()
    {
        Container.Bind<UIManager>().FromInstance(_uiManager).AsSingle();
    }
}