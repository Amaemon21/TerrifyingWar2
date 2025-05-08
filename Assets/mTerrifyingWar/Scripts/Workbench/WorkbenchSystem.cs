using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WorkbenchSystem : MonoBehaviour
{
    [Inject] private readonly DisplayProvider _displayProvider;
    
    [SerializeField] private FactoryRenderingItem _factoryRenderingItem;
    [SerializeField] private WorkbenchCell _cellPrefab;
    [SerializeField] private Transform _container;
    
    private List<WorkbenchCell> _cells = new ();
    
    private void OnEnable()
    {
        SpawnCell();
    }

    private void OnDisable()
    {
        ClearCells();
        _factoryRenderingItem.ClearsItems();
    }

    private void SpawnCell()
    {
        // _displayProvider.InventorySystem.RequestAllWeapons();
        
       //for (var i = 0; i < _displayProvider.InventorySystem.Weapons.Count; i++)
       //{
       //    var item = _displayProvider.InventorySystem.Weapons[i];
       //    
       //    WorkbenchCell cell = Instantiate(_cellPrefab, _container);
       //    cell.Setup(item, _factoryRenderingItem);
       //    _cells.Add(cell);
       //}
    }

    private void ClearCells()
    {
        foreach (var cell in _cells)
        {
            if (cell != null)
                Destroy(cell.gameObject);
        }

        _cells.Clear();
    }
}