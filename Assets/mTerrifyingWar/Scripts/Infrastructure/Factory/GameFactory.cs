using UnityEngine;

public class GameFactory : IGameFactory
{
    private readonly IAssetProvider _assetProvider;

    public GameFactory(IAssetProvider assetProvider)
    {
        _assetProvider = assetProvider;
    }

    private GameObject InstantiateRegistered(string prefabPath, Transform transform, Transform parent)
    {
        GameObject gameObject = _assetProvider.Instantiate(prefabPath, transform.position, Quaternion.identity, parent);
        return gameObject;
    }

    private GameObject InstantiateRegistered(string prefabPath, Transform parent)
    {
        GameObject gameObject = _assetProvider.Instantiate(prefabPath, parent);
        return gameObject;
    }
}