using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private InputService _inputService;

    public override void InstallBindings()
    {
        Container.Bind<PlayerController>().FromInstance(_playerController).AsSingle();
        Container.Bind<InputService>().FromInstance(_inputService).AsSingle();
    }
}