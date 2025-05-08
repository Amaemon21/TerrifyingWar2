using System;

[Serializable]
public class SaveData
{
    public string CreationDate { get; set; }

    public string Scene { get; set; } = Scenes.Gameplay;
    
    public PlayerEntity PlayerEntity {get; set;} = new();
    public InventoryEntity InventoryEntity {get; set;} = new();
}