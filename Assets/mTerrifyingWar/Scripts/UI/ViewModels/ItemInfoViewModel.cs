using MVVM;
using R3;
using UnityEngine;

public class ItemInfoViewModel : ViewModel
{
    private readonly ReactiveProperty<Sprite> _itemIcon = new();
    private readonly ReactiveProperty<string> _itemText = new();
    
    private readonly ItemInfo _itemInfo;
    
    public Observable<Sprite> ItemIcon => _itemIcon;
    public Observable<string> ItemText => _itemText;

    private ItemInfoViewModel(ItemInfo itemInfo)
    {
        _itemInfo = itemInfo;
    }
    
    public override void Initialize()
    {
        Disposable = _itemInfo.Config.Subscribe(UpdateView);
    }
    
    private void UpdateView(InventoryItemConfig inventoryItemConfig)
    {
        if (inventoryItemConfig == null)
            return;
        
        _itemIcon.Value = inventoryItemConfig.ItemSprite;
        
        if (inventoryItemConfig is WeaponInventoryItemConfig weaponInventoryItemConfig)
        {
            _itemText.Value =
                $"{inventoryItemConfig.ItemName}\n" +
                "<color=#E78300>-------------------</color>\n" +
                $"<line-height=90%>{inventoryItemConfig.ItemRarity}</line-height>\n" +
                "<color=#E78300>-------------------</cw" +
                "olor>\n" +
                $"<color=#E78300>Damage:</color> {weaponInventoryItemConfig.Damage}\n" +
                $"<color=#E78300>Fire Rate:</color> {weaponInventoryItemConfig.FireRate}\n" +
                $"<color=#E78300>Magazine Size:</color> {weaponInventoryItemConfig.MagazineSize}\n" +
                "<color=#E78300>-------------------</color>\n" +
                $"{inventoryItemConfig.ItemDescription}\n" +
                "<color=#E78300>-------------------</color>\n" +
                $"<color=#E78300>Strength:</color> {weaponInventoryItemConfig.Durability}";
        }
        else
        {
            _itemText.Value =
                $"{inventoryItemConfig.ItemName}\n" +
                "<color=#E78300>-------------------</color>\n" +
                $"<line-height=90%>{inventoryItemConfig.ItemRarity}</line-height>\n" +
                "<color=#E78300>-------------------</color>\n" +
                $"{inventoryItemConfig.ItemDescription}";
        }
    }
}