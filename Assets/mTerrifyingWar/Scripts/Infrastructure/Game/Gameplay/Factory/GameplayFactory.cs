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
    
    public void CreatePlayer(Vector3 position)
    {
        var plaeyrObject = InstantiateRegistered(AssetsPath.PlayerPath, position);
        PlayerContainer playerContainer = plaeyrObject.GetComponent<PlayerContainer>();
        _playerProvider.Setup(playerContainer);
    }

    public void CreateHud()
    {
        var hudObject = InstantiateRegistered(AssetsPath.UIPath);
        var displayContainer = hudObject.GetComponent<DisplayContainer>();
        _displayProvider.Setup(displayContainer);
        CreateHudChanged?.Invoke();
    }

    private GameObject InstantiateRegistered(string prefabPath, Vector3 position)
    {
        GameObject gameObject = _assetProvider.Instantiate(prefabPath, position, Quaternion.identity, null);
        return gameObject;
    }

    private GameObject InstantiateRegistered(string prefabPath)
    {
        GameObject gameObject = _assetProvider.Instantiate(prefabPath, null);
        return gameObject;
    }
}