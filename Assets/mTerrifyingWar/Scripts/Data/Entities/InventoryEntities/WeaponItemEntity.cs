using System;

[Serializable]
public class WeaponItemEntity : ItemEntity
{
    public int CurrentAmmo { get; set; } 
    public bool IsEquipped { get; set; } 
    
    public WeaponItemEntity()
    {
        EntityType = EntityType.WeaponItemEntity;
    }
}