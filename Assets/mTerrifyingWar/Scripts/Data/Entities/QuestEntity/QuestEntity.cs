using System;
using System.Collections.Generic;

[Serializable]
public class QuestEntity : Entity
{
    public string QuestId { get; set; }
    public List<QuestObjectiveEntity> QuestObjectiveEntities { get; set; } = new();
    public bool IsComplated { get; set; }
}

[Serializable]
public class QuestObjectiveEntity
{
    public int RequiredAmount{ get; set; }

    public QuestObjectiveEntity(int requiredAmount)
    {
        RequiredAmount = requiredAmount;
    }
}