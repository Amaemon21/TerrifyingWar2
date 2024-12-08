using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class ActionMenuObject : MonoBehaviour
{
    [Inject] private readonly Inventory _inventory;
    [Inject] private readonly InventoryManager _inventoryManager;

    [SerializeField] private Button _dropButton;
    [SerializeField] private Button _defuseWeaponButton;
    [SerializeField] private Button _closeButton;
    
    private InventoryDatabase _inventoryDatabase;
    private InventoryItemConfig _inventoryItemConfig;
    private InventoryItemCell _inventoryItemCell;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        _inventoryDatabase = _inventoryManager.InventoryDatabase;

        Hide();
    }

    public void SetupActionMenu(InventoryItemConfig inventoryItemConfig, InventoryItemCell cell, PointerEventData eventData)
    {
        SetupConfig(inventoryItemConfig);

        _inventoryItemCell = cell;
        
        SetupPosition(eventData);
    }
    
    public void DropItem()
    {
        _inventory.DropItem(_inventoryItemConfig, _inventoryItemCell);
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
                    var ammoInventoryItemConfig = _inventoryDatabase.FindItemByID(weaponInventoryItemConfig.AmmoID);
                    
                    var _ammoInventoryItemConfigCopy = Instantiate(ammoInventoryItemConfig);
                    
                    _ammoInventoryItemConfigCopy.ResetCount();
                    _ammoInventoryItemConfigCopy.AddCount(weaponInventoryItemConfig.CurrentAmmo);
                    
                    _inventory.AddItem(_ammoInventoryItemConfigCopy);
                
                    weaponInventoryItemConfig.ResetCurrentAmmo();
                }
            }
        }
        
        Hide();
    }

    private void SetupConfig(InventoryItemConfig inventoryItemConfig)
    {
        _inventoryItemConfig = inventoryItemConfig;

        _dropButton.gameObject.SetActive(true);
        _closeButton.gameObject.SetActive(true);
        
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