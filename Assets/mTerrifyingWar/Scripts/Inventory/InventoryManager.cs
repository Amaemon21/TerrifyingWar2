using NaughtyAttributes;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField, BoxGroup("Database"), HorizontalLine] private InventoryDatabase _inventoryDatabase;
    
    [SerializeField, BoxGroup("Utils"), HorizontalLine] private InventoryDragableObject _dragableObject;
    [SerializeField, BoxGroup("Utils")] private DropArea _dropArea;
    [SerializeField, BoxGroup("Utils")] private DropPosition _dropPosition;
    [SerializeField, BoxGroup("Utils")] private ItemInfo _itemInfo;
    [SerializeField, BoxGroup("Utils")] private ActionMenuObject _actionMenuObject;
    [SerializeField, BoxGroup("Utils")] private DropMenu _dropMenu;
    
    public InventoryDatabase InventoryDatabase => _inventoryDatabase;
    public InventoryDragableObject DragableObject => _dragableObject;
    public DropArea DropArea => _dropArea;
    public DropPosition DropPosition => _dropPosition;
    public ItemInfo ItemInfo => _itemInfo;
    public ActionMenuObject ActionMenuObject => _actionMenuObject;
    public DropMenu DropMenu => _dropMenu;
}
