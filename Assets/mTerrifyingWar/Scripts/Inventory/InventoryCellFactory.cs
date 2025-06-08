using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class InventoryCellFactory : MonoBehaviour
{
    [Inject] private readonly DiContainer _container;
    
    [SerializeField, BoxGroup("Common"), HorizontalLine] private RectTransform _cellInventoryContainer;
    [SerializeField, BoxGroup("Common")] private RectTransform _cellLootContainer;
    [SerializeField, BoxGroup("Common")] private int _cellInventoryCount = 36;
    [SerializeField, BoxGroup("Common")] private int _cellLootCount = 6;
    [SerializeField, BoxGroup("Common")] private InventoryItemCell _cellPrefab;
    
    public void SpawnInventoryCells(List<InventoryItemCell> inventoryCells)
    {
        RecreateCells(inventoryCells, _cellInventoryCount, _cellInventoryContainer);
    }

    public void SpawnLootCells(List<InventoryItemCell> lootCells)
    {
        RecreateCells(lootCells, _cellLootCount, _cellLootContainer);
    }

    private void RecreateCells(List<InventoryItemCell> targetList, int count, RectTransform parent)
    {
        ClearCells(targetList);

        for (int i = 0; i < count; i++)
        {
            InventoryItemCell cell = _container.InstantiatePrefabForComponent<InventoryItemCell>(_cellPrefab, parent);
            cell.name = $"Slot: {i + 1}";
            targetList.Add(cell);
        }
    }

    private void ClearCells(List<InventoryItemCell> cells)
    {
        if (cells == null) return;

        foreach (var cell in cells)
        {
            if (cell != null)
                Destroy(cell.gameObject);
        }

        cells.Clear();
    }
}
