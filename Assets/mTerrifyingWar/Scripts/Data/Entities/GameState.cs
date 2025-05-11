using System;
using System.Collections.Generic;

[Serializable]
public class GameState
{
    public string CreationDate { get; set; }

    public string Scene { get; set; } = Scenes.Gameplay;

    public int LastId { get; set; } = 0;

    public List<Entity> Entities { get; set; } = new();

    public int GetNewId()
    {
        return ++LastId;
    }
}