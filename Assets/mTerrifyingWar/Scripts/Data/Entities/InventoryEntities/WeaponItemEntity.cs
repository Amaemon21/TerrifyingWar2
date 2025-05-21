using System;

[Serializable]
public class WeaponItemEntity : ItemEntity
{
    public int CurrentAmmo { get; set; } 
    public bool IsEquipped { get; set; } 
    public SlotType SlotType { get; set; }
    
    public WeaponItemEntity()
    {
        EntityType = EntityType.WeaponItemEntity;
    }
}

public enum SlotType
{
    Primary,
    Secondary,
}