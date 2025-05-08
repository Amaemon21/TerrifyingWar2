using UnityEngine;
using Zenject;

public class AssetsProvider : IAssetsProvider
{
    private readonly DiContainer _container;
    
    public AssetsProvider(DiContainer container)
    {
        _container = container;
    }
    
    public PlayerContainer PlayerInstantiate(Transform transform)
    {
        GameObject prefab = Resources.Load<GameObject>(AssetsPath.PlayerPath);
        PlayerContainer playerContainer = _container.InstantiatePrefabForComponent<PlayerContainer>(prefab, transform.position, Quaternion.identity, null);
        return playerContainer;
    }

    public DisplayContainer UIInstantiate()
    {
        GameObject prefab = Resources.Load<GameObject>(AssetsPath.UIPath);
        DisplayContainer displayContainer = _container.InstantiatePrefabForComponent<DisplayContainer>(prefab);
        return displayContainer;
    }
}