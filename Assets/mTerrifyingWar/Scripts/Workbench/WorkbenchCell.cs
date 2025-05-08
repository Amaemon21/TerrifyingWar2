using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorkbenchCell : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _nameText;
    
    private WeaponInventoryItemConfig _weaponInventoryItemConfig;
    private FactoryRenderingItem _factoryRenderingItem;
    
    public void Setup(WeaponInventoryItemConfig weaponInventoryItemConfig, FactoryRenderingItem factoryRendering)
    {
        _weaponInventoryItemConfig = weaponInventoryItemConfig;
        
        _nameText.text = _weaponInventoryItemConfig.ItemName;
        _icon.sprite = _weaponInventoryItemConfig.EquippedSprite;
        
        _icon.enabled = true;
        
        _factoryRenderingItem = factoryRendering;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _factoryRenderingItem.SpawnItem(_weaponInventoryItemConfig);
        }
    }
}