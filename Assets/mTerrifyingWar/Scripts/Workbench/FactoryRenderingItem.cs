using System.Collections.Generic;
using UnityEngine;

public class FactoryRenderingItem : MonoBehaviour
{
    private List<Weapon> _renderingItems = new();
    private HashSet<Weapon> _usedPrefabs = new();

    public void SpawnItem(WeaponInventoryItemConfig weaponInventoryItemConfig)
    {
        var prefab = weaponInventoryItemConfig.WeaponHandPrefab;

        if (_usedPrefabs.Contains(prefab))
            return;

        var instance = Instantiate(prefab, transform);
        _renderingItems.Add(instance);
        _usedPrefabs.Add(prefab);
    }

    public void ClearsItems()
    {
        foreach (var item in _renderingItems)
        {
            if (item != null)
                Destroy(item.gameObject);
        }

        _renderingItems.Clear();
        _usedPrefabs.Clear();
    }
}