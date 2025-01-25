using System;
using UnityEngine;

public class GameplayFactory : IGameplayFactory
{
    private readonly IAssetProvider _assetProvider;
    private readonly PlayerProvider _playerProvider;
    private readonly DisplayProvider _displayProvider;
    
    public event Action CreateHudChanged;
    
    public GameplayFactory(IAssetProvider assetProvider, PlayerProvider playerProvider, DisplayProvider displayProvider)
    {
        _assetProvider = assetProvider;
        _playerProvider = playerProvider;
        _displayProvider = displayProvider;
    }
    
    public void CreatePlayer(Transform spawnTransform)
    {
        var plaeyrObject = InstantiateRegistered(AssetsPath.PlayerPath, spawnTransform);
        var playerContainer = plaeyrObject.GetComponent<PlayerContainer>();
        _playerProvider.Setup(playerContainer);
    }

    public void CreateHud()
    {
        var hudObject = InstantiateRegistered(AssetsPath.UIPath);
        var displayContainer = hudObject.GetComponent<DisplayContainer>();
        _displayProvider.Setup(displayContainer);
        CreateHudChanged?.Invoke();
    }

    private GameObject InstantiateRegistered(string prefabPath, Transform transform)
    {
        GameObject gameObject = _assetProvider.Instantiate(prefabPath, transform, null);
        return gameObject;
    }

    private GameObject InstantiateRegistered(string prefabPath)
    {
        GameObject gameObject = _assetProvider.Instantiate(prefabPath, null);
        return gameObject;
    }
}