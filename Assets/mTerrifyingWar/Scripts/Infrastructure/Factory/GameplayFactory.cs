using System;
using UnityEngine;

public class GameplayFactory : IGameplayFactory
{
    private readonly IAssetsProvider _assetsProvider;
    private readonly PlayerProvider _playerProvider;
    private readonly DisplayProvider _displayProvider;
    
    public event Action CreatePlayerChanged;
    public event Action CreateHudChanged;
    
    public GameplayFactory(IAssetsProvider assetsProvider, PlayerProvider playerProvider, DisplayProvider displayProvider)
    {
        _assetsProvider = assetsProvider;
        _playerProvider = playerProvider;
        _displayProvider = displayProvider;
    }
    
    public void CreatePlayer(Transform spawnTransform)
    {
        var playerContainer = _assetsProvider.PlayerInstantiate(spawnTransform);
        _playerProvider.Setup(playerContainer);
        CreatePlayerChanged?.Invoke();
    }

    public void CreateHud()
    {
        var displayProvider = _assetsProvider.UIInstantiate();
        _displayProvider.Setup(displayProvider);
        CreateHudChanged?.Invoke();
    }
}