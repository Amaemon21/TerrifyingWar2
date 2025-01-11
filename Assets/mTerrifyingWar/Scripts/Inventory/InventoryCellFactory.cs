using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class InventoryCellFactory : MonoBehaviour
{
    [Inject] private readonly DiContainer _container;
    
    [SerializeField, BoxGroup("Common"), HorizontalLine] private RectTransform _cellContainer;
    [SerializeField, BoxGroup("Common")] private int _cellCount = 36;
    [SerializeField, BoxGroup("Common")] private InventoryItemCell _cellPrefab;
    
    public void SpawnCells(List<InventoryItemCell> inventoryItemCells)
    {
        inventoryItemCells.Clear();

        for (int i = 0; i < _cellCount; i++)
        {
            InventoryItemCell cell = _container.InstantiatePrefabForComponent<InventoryItemCell>(_cellPrefab, _cellContainer);
            cell.name = $"Slot: {i + 1}";
                        
            inventoryItemCells.Add(cell);
        }
    }
}
