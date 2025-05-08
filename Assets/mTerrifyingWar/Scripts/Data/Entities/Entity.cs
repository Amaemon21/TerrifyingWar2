public class Entity
{
    public EntityType EntityType { get; set; }
}

public enum EntityType
{
    ItemEntity,
    WeaponItemEntity
}