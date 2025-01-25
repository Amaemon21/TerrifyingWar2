using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class InventoryItemEquipableCell : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [Inject] private readonly DisplayProvider _displayProvider;
    [Inject] private readonly ItemInfo _itemInfo;
    
    private readonly string _nameItemDefault = "Отсутствует";

    [SerializeField] private ItemType _cellType = ItemType.Weapon;
    
    [Space(10)]
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private Image _cellImage;
    [SerializeField] private Image _iconImage;
    [SerializeField] private Image _dropImage;
    
    [Space(10)]
    [SerializeField] private Sprite _emptyCellSprite;
    
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
    private ItemInfoView ItemInfoView;

    public InventoryItemConfig InventoryItemConfig { get; private set; }

    public event Action DropItemChanged; 

    private void Awake()
    {
        _dragableObject = _displayProvider.Inventory.DragableObject;
        _dropArea = _displayProvider.Inventory.DropArea;
        ItemInfoView = _displayProvider.Inventory.ItemInfoView;
    }
    
    private void OnEnable()
    {
        RedrawCell();
    }

    private void OnDisable()
    {
        DisplayCellIconByItemType();
        
        ItemInfoView.gameObject.SetActive(false);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (InventoryItemConfig != null)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                ItemInfoView.gameObject.SetActive(false);
            }
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        DisplayCellIconByItemType(true);
        
        if (InventoryItemConfig != null)
        {
            ItemInfoView.gameObject.SetActive(true);
            _itemInfo.Setup(InventoryItemConfig);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DisplayCellIconByItemType();
        
        ItemInfoView.gameObject.SetActive(false);
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

            if (InventoryItemConfig.ItemType == ItemType.Weapon)
            {
                if (InventoryItemConfig is WeaponInventoryItemConfig weaponInventoryItemConfig)
                {
                    if (weaponInventoryItemConfig != null)
                    {
                        _iconImage.sprite = weaponInventoryItemConfig.EquippedSprite;

                        _nameText.text = $"<color=#E78300>{weaponInventoryItemConfig.ItemName}</color>" ;
                            
                        _dropImage.enabled = false;
                    }
                }
            }
            else
            {
                _iconImage.sprite = InventoryItemConfig.ItemSprite;
                
                _nameText.text = $"<color=#E78300>{InventoryItemConfig.ItemName}</color>" ;
                            
                _dropImage.enabled = false;
            }

            DisplayCellIconByItemType();
        }
        else
        {
            _iconImage.enabled = false;

            _dropImage.enabled = true;
            
            _nameText.text = string.Empty;
            _nameText.text = $"<color=#ff0000>{_nameItemDefault}</color>" ;
            
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
            _dragableObject.gameObject.SetActive(true);
            _dropArea.Show();
        
            _dragableObject.SetupEquipableCell(this);

            DropItemChanged?.Invoke();
            
            _dragableObject.Icon.sprite = InventoryItemConfig.ItemSprite;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        _dragableObject.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (InventoryItemConfig != null)
        {
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<DropArea>())
                {
                    string count = InventoryItemConfig.ItemCount > 1 ? $" : x{Utils.FormatNumber(InventoryItemConfig.ItemCount, '.')}" : string.Empty;

                    Debug.Log($"We dropped item to world: <color=#E78300>{InventoryItemConfig.ItemName}{count}</color>");
                    
                    _displayProvider.Inventory.DropItem(InventoryItemConfig);
                }
            }
        }
        
        _dragableObject.gameObject.SetActive(false);
        
        DropItemChanged?.Invoke();
        ItemInfoView.gameObject.SetActive(false);
        _dropArea.Hide();
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        if (_dragableObject == null)
            return;

        InventoryItemConfig draggedItemConfig = null;
        
        if (_dragableObject.InventoryItemCell != null)
        {
            draggedItemConfig = _dragableObject.InventoryItemCell.InventoryItemConfig;

            if (draggedItemConfig != null && draggedItemConfig.ItemType == _cellType)
            {
                if (InventoryItemConfig != null) 
                {
                    _dragableObject.InventoryItemCell.SetItem(InventoryItemConfig);
                    _dragableObject.SetupCell(_dragableObject.InventoryItemCell);
                    InventoryItemConfig = draggedItemConfig;
                }
                else
                {
                    _dragableObject.InventoryItemCell.SetItem(null);
                    InventoryItemConfig = draggedItemConfig;
                    _dragableObject.SetupCell(null);
                }
            }
            else
            {
                _dragableObject.SetupCell(null);
                return;
            }
        }
        else if (_dragableObject.InventoryItemEquipableCell != null)
        {
            draggedItemConfig = _dragableObject.InventoryItemEquipableCell.InventoryItemConfig;

            if (draggedItemConfig != null && draggedItemConfig.ItemType == _cellType)
            {
                if (InventoryItemConfig != null) // Выполняем свап
                {
                    _dragableObject.InventoryItemEquipableCell.SetItem(InventoryItemConfig);
                    _dragableObject.SetupEquipableCell(_dragableObject.InventoryItemEquipableCell);
                    InventoryItemConfig = draggedItemConfig;
                    DropItemChanged?.Invoke();
                }
                else
                {
                    _dragableObject.InventoryItemEquipableCell.SetItem(null);
                    InventoryItemConfig = draggedItemConfig;
                    _dragableObject.SetupEquipableCell(null);
                }
            }
            else
            {
                _dragableObject.SetupEquipableCell(null);
                return;
            }
        }

        if (draggedItemConfig != null)
        {
            RedrawCell();
        
            DropItemChanged?.Invoke();
        
            AnimateDrop();
        }
    }
    
    private void AnimateDrop()
    {   
        transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 10, 1);
    }
}