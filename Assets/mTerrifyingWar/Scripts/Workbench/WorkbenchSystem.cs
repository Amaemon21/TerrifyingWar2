using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WorkbenchSystem : MonoBehaviour
{
    [Inject] private readonly DisplayProvider _displayProvider;
    
    [SerializeField] private WorkbenchCell _cellPrefab;
    [SerializeField] private Transform _container;
    
    private List<WorkbenchCell> _cells = new();

    private WorkbenchInteractObject _workbenchInteractObject;
    
    public void Setup(WorkbenchInteractObject workbenchInteractObject)
    {
        _workbenchInteractObject = workbenchInteractObject;
    }
    
    private void OnEnable()
    {
        SpawnCell();
    }

    private void OnDisable()
    {
        ClearCells();
        _workbenchInteractObject.FactoryWeaponItem.ClearsItems();
        
        _workbenchInteractObject.Exit();
    }

    private void SpawnCell()
    {
        for (var i = 0; i < _displayProvider.Inventory.InventoryComponent.InventorySystem.Weapons.Count; i++)
        {
            var item = _displayProvider.Inventory.InventoryComponent.InventorySystem.Weapons[i];
            
            WorkbenchCell cell = Instantiate(_cellPrefab, _container);
            cell.Setup(item, _workbenchInteractObject.FactoryWeaponItem);
            _cells.Add(cell);
        }
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