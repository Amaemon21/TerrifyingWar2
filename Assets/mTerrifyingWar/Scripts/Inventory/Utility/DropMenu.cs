using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DropMenu : MonoBehaviour
{
    [Inject] private readonly DisplayProvider _displayProvider;
    
    [SerializeField] private Image _iconItem;
    [SerializeField] private TMP_Text _maxDropCountText;

    [SerializeField] private Slider _dropSlider;

    [SerializeField] private TMP_Text _currentDropCountText;
    
    private InventoryItemConfig _inventoryItemConfig;
    private InventoryItemCell _inventoryItemCell;
    
    private int _value;

    private void Awake()
    {
        _dropSlider.minValue = 1;
    }

    public void ValueChanged()
    {
        _value = (int)_dropSlider.value;
        _currentDropCountText.text = $"x{_value}";
    }

    public void Setup(InventoryItemConfig config, InventoryItemCell inventoryItemCell)
    {
        _inventoryItemConfig = config;
        _inventoryItemCell = inventoryItemCell;
        
        _iconItem.sprite = _inventoryItemConfig.ItemSprite;
        _maxDropCountText.text = $"x{_inventoryItemConfig.ItemCount}";

        _dropSlider.maxValue = _inventoryItemConfig.ItemCount;
        _dropSlider.value = _inventoryItemConfig.ItemCount / 2;
    }

    public void DropItem()
    {
        _displayProvider.Inventory.DropItem(_inventoryItemConfig, _value, _inventoryItemCell);
        _displayProvider.Inventory.DisplayItems();
        Close();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
