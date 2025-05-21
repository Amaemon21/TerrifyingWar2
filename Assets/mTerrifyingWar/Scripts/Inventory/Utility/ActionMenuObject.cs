using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class ActionMenuObject : MonoBehaviour
{
    [Inject] private readonly DisplayProvider _displayProvider;
    [Inject] private readonly PlayerHealth _playerHealth;

    [SerializeField] private Button _useButton;
    [SerializeField] private Button _dropButton;
    [SerializeField] private Button _dropAllButton;
    [SerializeField] private Button _defuseWeaponButton;
    [SerializeField] private Button _closeButton;
    
    private InventoryDatabase _inventoryDatabase;
    private InventoryItemConfig _inventoryItemConfig;
    private InventoryItemCell _inventoryItemCell;
    private DropMenu _dropMenu;
    private RectTransform _rectTransform;

    private void Awake()
    {
        gameObject.SetActive(false);
        
        _rectTransform = GetComponent<RectTransform>();

        _inventoryDatabase = _displayProvider.Inventory.InventoryComponent.InventoryDatabase;
        _dropMenu = _displayProvider.Inventory.InventoryComponent.DropMenu;

        Hide();
    }

    public void SetupActionMenu(InventoryItemConfig inventoryItemConfig, InventoryItemCell cell, PointerEventData eventData)
    {
        SetupConfig(inventoryItemConfig);

        _inventoryItemCell = cell;
        
        SetupPosition(eventData);
        
        gameObject.SetActive(true);
    }
    
    public void UseItem()
    {
        _inventoryItemConfig.Use(_playerHealth);
        _displayProvider.Inventory.RemoveItem(_inventoryItemConfig);
        Hide();
    }
    
    public void DropItem()
    {
        _displayProvider.Inventory.DropItem(_inventoryItemConfig, _inventoryItemCell);
        Hide();
    }

    public void DropAllItem()
    {
        if (_inventoryItemConfig.ItemCount > 1)
        {
            _dropMenu.gameObject.SetActive(true);
            _dropMenu.Setup(_inventoryItemConfig, _inventoryItemCell);
        }
        else
        {
            _displayProvider.Inventory.DropItem(_inventoryItemConfig, _inventoryItemCell);
        }

        Hide();
    }
    
    public void DefuseWeaponItem()
    {
        if (_inventoryItemConfig is WeaponInventoryItemConfig weaponInventoryItemConfig)
        {
            if (weaponInventoryItemConfig != null)
            {
                if (weaponInventoryItemConfig.CurrentAmmo > 1)
                {
                    InventoryItemConfig ammoInventoryItemConfig = _inventoryDatabase.FindItemByID(_inventoryItemConfig.ItemID);
                    
                    InventoryItemConfig _ammoInventoryItemConfigCopy = Instantiate(ammoInventoryItemConfig);
                    
                    _ammoInventoryItemConfigCopy.ResetCount();
                    _ammoInventoryItemConfigCopy.AddCount(weaponInventoryItemConfig.CurrentAmmo);
                    
                    _displayProvider.Inventory.AddItem(_ammoInventoryItemConfigCopy);
                
                    weaponInventoryItemConfig.ResetCurrentAmmo();
                }
            }
        }
        
        Hide();
    }

    private void SetupConfig(InventoryItemConfig inventoryItemConfig)
    {
        _inventoryItemConfig = inventoryItemConfig;

        if (inventoryItemConfig is MedicationsItemConfig medicationsItemConfig)
        {
            _useButton.gameObject.SetActive(true);
        }
        else
        {
            _useButton.gameObject.SetActive(false);
        }
        
        _dropButton.gameObject.SetActive(true);
        _closeButton.gameObject.SetActive(true);

        _dropAllButton.gameObject.SetActive(inventoryItemConfig.ItemCount > 1);

        if (inventoryItemConfig is WeaponInventoryItemConfig weaponInventoryItemConfig)
        {
            _defuseWeaponButton.gameObject.SetActive(weaponInventoryItemConfig.CurrentAmmo > 0);
        }
        else
        {
            _defuseWeaponButton.gameObject.SetActive(false);
        }
    }
    
    private void SetupPosition(PointerEventData eventData)
    {
        float offsetX = _rectTransform.rect.width / 2;
        float offsetY = -1 * (_rectTransform.rect.height / 2);

        Vector2 cursorPosition = eventData.position;

        Vector2 newPosition = cursorPosition + new Vector2(offsetX, offsetY);

        transform.position = newPosition;
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}