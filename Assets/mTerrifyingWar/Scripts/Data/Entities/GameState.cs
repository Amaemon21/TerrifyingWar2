using System;
using System.Collections.Generic;

[Serializable]
public class GameState
{
    public string CreationDate { get; set; }

    public int LastId { get; set; } = 0;

    public PlayerEntity PlayerEntity { get; set; }
    
    public List<Entity> Entities { get; set; } = new();

    public int GetNewId()
    {
        return ++LastId;
    }
}