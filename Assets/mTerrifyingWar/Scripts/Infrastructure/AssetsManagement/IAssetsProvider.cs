using UnityEngine;

public interface IAssetsProvider : IService
{
    public PlayerContainer PlayerInstantiate(Transform transform);
    public DisplayContainer UIInstantiate();
    public Weapon WeaponInstantiate(Weapon handPrefab, Transform parent);
}