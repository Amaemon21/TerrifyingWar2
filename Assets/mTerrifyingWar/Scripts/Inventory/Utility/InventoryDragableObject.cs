using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDragableObject : MonoBehaviour
{
    [field: SerializeField] public Image Icon { get; private set; }

    public InventoryItemCell InventoryItemCell { get; private set; }
    public InventoryItemEquipableCell InventoryItemEquipableCell { get; private set; }

    public void SetupCell(InventoryItemCell cell) => InventoryItemCell = cell;
    public void SetupEquipableCell(InventoryItemEquipableCell cell) => InventoryItemEquipableCell = cell;

    private void OnEnable()
    {
        transform.DOScale(1.15f, 0.1f).SetLink(transform.gameObject);
    }

    private void OnDisable()
    {
        transform.DOScale(1f, 0.1f).SetLink(transform.gameObject);
    }
}