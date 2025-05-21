public class Entity
{
    public int EntityId { get; set; }
    public EntityType EntityType { get; set; }
}

public enum EntityType
{
    ItemEntity,
    WeaponItemEntity,
    QuestEntity
}