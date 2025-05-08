using System.Collections.Generic;

[System.Serializable]
public class QuestStatus
{
    public QuestConfig QuestConfig;
    public Dictionary<string, int> Progress = new();

    public bool IsCompleted()
    {
        foreach (var objective in QuestConfig.Objectives)
        {
            if (!Progress.ContainsKey(objective.Id) || Progress[objective.Id] < objective.RequiredAmount)
                return false;
        }
        
        return true;
    }

    public void AddProgress(string objectiveId, int amount = 1)
    {
        if (!Progress.ContainsKey(objectiveId))
            Progress[objectiveId] = 0;

        Progress[objectiveId] += amount;
    }
}