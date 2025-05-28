using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorkbenchCell : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _nameText;
    
    private WeaponInventoryItemConfig _weaponInventoryItemConfig;
    private FactoryWeaponItem factoryWeaponItem;
    
    public void Setup(WeaponInventoryItemConfig weaponInventoryItemConfig, FactoryWeaponItem factoryWeapon)
    {
        _weaponInventoryItemConfig = weaponInventoryItemConfig;
        
        _nameText.text = _weaponInventoryItemConfig.ItemName;
        _icon.sprite = _weaponInventoryItemConfig.EquippedSprite;
        
        _icon.enabled = true;
        
        factoryWeaponItem = factoryWeapon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            factoryWeaponItem.SpawnItem(_weaponInventoryItemConfig);
        }
    }
}