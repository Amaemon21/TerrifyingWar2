using System;

[Serializable]
public class ItemEntity : Entity
{
    public string ItemId { get; set; } 
    public int Count { get; set; } 

    public ItemEntity()
    {
        EntityType = EntityType.ItemEntity;
    }
}