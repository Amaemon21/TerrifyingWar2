using UnityEngine;
using Zenject;

public class AssetProvider : IAssetProvider
{
    private readonly DiContainer _container;
    
    public AssetProvider(DiContainer container)
    {
        _container = container;
    }
    
    public GameObject Instantiate(string path, Transform parent)
    {
        GameObject prefab = Resources.Load<GameObject>(path);
        var gameObject = _container.InstantiatePrefab(prefab, parent);
        return gameObject;
    }
    
    public GameObject Instantiate(string path, Transform transform, Transform parent)
    {
        GameObject prefab = Resources.Load<GameObject>(path);
        GameObject gameObject = _container.InstantiatePrefab(prefab, transform.position, transform.rotation, parent);

        return gameObject;
    }
}