using System;

[Serializable]
public class PlayerEntity : Entity
{
    public float MaxHealth {get; set;}
    public float Health {get; set;}

    public PlayerEntity()
    {
        MaxHealth = 100f;
        Health = MaxHealth;
        EntityType = EntityType.PlayerEntity;
    }
}