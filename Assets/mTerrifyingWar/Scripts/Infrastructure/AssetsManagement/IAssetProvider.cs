using UnityEngine;

public interface IAssetProvider : IService
{
    public GameObject Instantiate(string path, Transform parent);
    public GameObject Instantiate(string path, Transform transform, Transform parent);
}