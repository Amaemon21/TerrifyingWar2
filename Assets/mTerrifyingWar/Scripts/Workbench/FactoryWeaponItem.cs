using System.Collections.Generic;
using UnityEngine;

public class FactoryWeaponItem : MonoBehaviour
{
    [SerializeField] private Transform _weaponParent;
    
    private List<InventoryItemObject> _renderingItems = new();
    private HashSet<InventoryItemObject> _usedPrefabs = new();

    public void SpawnItem(WeaponInventoryItemConfig weaponInventoryItemConfig)
    {
        InventoryItemObject prefab = weaponInventoryItemConfig.ItemPrefab;

        if (_usedPrefabs.Contains(prefab))
            return;

        InventoryItemObject instance = Instantiate(prefab, _weaponParent.position, _weaponParent.rotation, _weaponParent);
        instance.Rigidbody.isKinematic = true;
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