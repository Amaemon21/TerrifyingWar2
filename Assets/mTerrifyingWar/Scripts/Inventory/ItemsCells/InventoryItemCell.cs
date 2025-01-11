using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class InventoryItemCell : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [Inject] private readonly Inventory _inventory;
    
    [SerializeField] protected Image _cellImage;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private Image _iconImage;

    [Space(10)]
    [SerializeField] protected Sprite _emptyCellSprite;
    
    [Space]
    [SerializeField] private Sprite _nonEmptyCellSprite;
    [SerializeField] private Sprite _nonEmptyRareCellSprite;
    [SerializeField] private Sprite _nonEmptyMythicalCellSprite;
    
    [Space]
    [SerializeField] private Sprite _highlightedCellSprite;
    [SerializeField] private Sprite _highlightedRareCellSprite;
    [SerializeField] private Sprite _highlightedMythicalCellSprite;
    
    private InventoryDragableObject _dragableObject;
    private DropArea _dropArea;
    private ItemInfo _itemInfo;
    private ActionMenuObject _actionMenuObject;
    private DropMenu _dropMenu;
    
    public InventoryItemConfig InventoryItemConfig { get; private set; }
    
    private void Awake()
    {
        _dragableObject = _inventory.DragableObject;
        _dropArea = _inventory.DropArea;
        _itemInfo = _inventory.ItemInfo;
        _actionMenuObject = _inventory.ActionMenuObject;
        _dropMenu = _inventory.DropMenu;
    }

    private void OnEnable()
    {
        RedrawCell();
    }
    
    private void OnDisable()
    {
        DisplayCellIconByItemType();
        _itemInfo.gameObject.SetActive(false);
        _actionMenuObject.gameObject.SetActive(false);
        _dropMenu.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (InventoryItemConfig != null)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _actionMenuObject.gameObject.SetActive(false);
            }

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                _itemInfo.gameObject.SetActive(false);
                
                _actionMenuObject.gameObject.SetActive(true);
                _actionMenuObject.SetupActionMenu(InventoryItemConfig, this, eventData);
            }
        }
        else
        {
            _actionMenuObject.gameObject.SetActive(false);
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        DisplayCellIconByItemType(true);

        if (InventoryItemConfig != null)
        {
            _itemInfo.gameObject.SetActive(true);
            _itemInfo.SetConfig(InventoryItemConfig);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DisplayCellIconByItemType();

        _itemInfo.gameObject.SetActive(false);
    }
    
    public void SetItem(InventoryItemConfig config)
    {
        InventoryItemConfig = config;
        RedrawCell();
    }

    public void RedrawCell()
    {
        if (InventoryItemConfig != null)
        {
            _iconImage.enabled = true;

            _iconImage.sprite = InventoryItemConfig.ItemSprite;

            DisplayCellIconByItemType();

            if (InventoryItemConfig.ItemCount > 1)
            {
                _countText.text = $"{Utils.FormatNumber(InventoryItemConfig.ItemCount, '.')}";
            }
            else
            {
                _countText.text = string.Empty;
            }
        }
        else
        {
            _iconImage.enabled = false;
            
            _countText.text = string.Empty;

            DisplayCellIconByItemType(_emptyCellSprite);
        }
    }
    
    private void DisplayCellIconByItemType(bool highlighted = false)
    {
        if (InventoryItemConfig != null)
        {
            switch (InventoryItemConfig.ItemRarity)
            {
                case RarityType.Normal:
                    _cellImage.sprite = highlighted ? _highlightedCellSprite : _nonEmptyCellSprite;
                    break;
                case RarityType.Rare:
                    _cellImage.sprite = highlighted ? _highlightedRareCellSprite : _nonEmptyRareCellSprite;
                    break;
                case RarityType.Legendary:
                    _cellImage.sprite = highlighted ? _highlightedMythicalCellSprite : _nonEmptyMythicalCellSprite;
                    break;
            }
        }
        else
        {
            _cellImage.sprite = _emptyCellSprite;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (InventoryItemConfig != null)
        {
            _actionMenuObject.gameObject.SetActive(false);
            
            _dragableObject.gameObject.SetActive(true);
            
            _dropArea.Show();
        
            _dragableObject.SetupCell(this);

            _dragableObject.Icon.sprite = InventoryItemConfig.ItemSprite;
        }
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        _dragableObject.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _dragableObject.gameObject.SetActive(false);
        _dragableObject.SetupCell(null);

        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            if (eventData.pointerCurrentRaycast.gameObject.GetComponent<DropArea>())
            {
                if (InventoryItemConfig != null)
                {
                    if (InventoryItemConfig.ItemCount > 1)
                    {
                        _dropMenu.gameObject.SetActive(true);
                        _dropMenu.Setup(InventoryItemConfig, this);
                    }
                    else
                    {
                        _inventory.DropItem(InventoryItemConfig, this);
                    }
                }
            }
        }
        
        _dropArea.Hide();
        _itemInfo.gameObject.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItemConfig cachedConfig = InventoryItemConfig;

        if (_dragableObject.InventoryItemCell != null)
        {
            InventoryItemConfig = _dragableObject.InventoryItemCell.InventoryItemConfig;
            _dragableObject.InventoryItemCell.InventoryItemConfig = cachedConfig;
            _dragableObject.SetupCell(null);
        }
        else if(_dragableObject.InventoryItemEquipableCell != null)
        {
            if (InventoryItemConfig == null)
            {
                InventoryItemConfig = _dragableObject.InventoryItemEquipableCell.InventoryItemConfig;
                _dragableObject.InventoryItemEquipableCell.SetItem(null);
                _dragableObject.SetupEquipableCell(null);
            }
        }

        _inventory.DisplayItems();
    }
}