using System;
using UnityEngine;

public class GameplayFactory : IGameplayFactory
{
    private readonly IAssetsProvider _assetsProvider;
    private readonly IGameFactory _gameFactory;
    private readonly PlayerProvider _playerProvider;
    private readonly DisplayProvider _displayProvider;

    public event Action CreatePlayerChanged;
    public event Action CreateHudChanged;
    
    public GameplayFactory(IAssetsProvider assetsProvider, IGameFactory gameFactory, PlayerProvider playerProvider, DisplayProvider displayProvider)
    {
        _assetsProvider = assetsProvider;
        _gameFactory = gameFactory;
        _playerProvider = playerProvider;
        _displayProvider = displayProvider;
    }
    
    public void CreatePlayer(Transform spawnTransform)
    {
        PlayerContainer playerContainer = _assetsProvider.PlayerInstantiate(spawnTransform);
        _playerProvider.Setup(playerContainer);
        _gameFactory.RegisterProgressWatchers(playerContainer.gameObject);
        CreatePlayerChanged?.Invoke();
    }

    public void CreateHud()
    {
        DisplayContainer displayContainer = _assetsProvider.UIInstantiate();
        _displayProvider.Setup(displayContainer);
        _gameFactory.RegisterProgressWatchers(displayContainer.gameObject);
        CreateHudChanged?.Invoke();
    }

    public Weapon CreateWeapon(Weapon weaponHandPrefab, Transform parent)
    {
        Weapon weapon = _assetsProvider.WeaponInstantiate(weaponHandPrefab, parent);
        _gameFactory.RegisterProgressWatchers(weapon.gameObject);
        return weapon;
    }
}