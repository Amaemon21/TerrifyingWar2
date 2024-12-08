using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _infoText;

    private InventoryItemConfig _inventoryItemConfig;

    private void Awake()
    {
        gameObject.SetActive(true);
    }

    public void SetConfig(InventoryItemConfig inventoryItemConfig)
    {
        _inventoryItemConfig = inventoryItemConfig;

        Setup(_inventoryItemConfig);
    }

    private void Setup(InventoryItemConfig inventoryItemConfig)
    {
        if (inventoryItemConfig is WeaponInventoryItemConfig weaponInventoryItemConfig)
        {
            _icon.sprite = weaponInventoryItemConfig.ItemSprite;

            string text = "" +
                          $"{inventoryItemConfig.ItemName}\n" +
                          "<color=#E78300>──────────────────</color>\n" +
                          $"<line-height=90%>{inventoryItemConfig.ItemRarity}</line-height>\n" +
                          "<color=#E78300>──────────────────</color>\n" +
                          $"<color=#E78300>Damage:</color> {weaponInventoryItemConfig.Damage}\n" +
                          $"<color=#E78300>Fire Rate:</color> {weaponInventoryItemConfig.FireRate}\n" +
                          $"<color=#E78300>Magazine Size:</color> {weaponInventoryItemConfig.MagazineSize}\n" +
                          "<color=#E78300>──────────────────</color>\n" +
                          $"{inventoryItemConfig.ItemDescription}\n" +
                          "<color=#E78300>──────────────────</color>\n" +
                          $"<color=#E78300>Прочность:</color> {weaponInventoryItemConfig.Durability}";

            _infoText.text = text;
        }
        else
        {
            _icon.sprite = inventoryItemConfig.ItemSprite;
            
            string text = "" +
                          $"{inventoryItemConfig.ItemName}\n" +
                          "<color=#E78300>──────────────────</color>\n" +
                          $"<line-height=90%>{inventoryItemConfig.ItemRarity}</line-height>\n" +
                          "<color=#E78300>──────────────────</color>\n" +
                          $"{inventoryItemConfig.ItemDescription}";
            
            _infoText.text = text;
        }
    }
}