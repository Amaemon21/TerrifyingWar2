using System.Collections.Generic;
using UnityEngine;

public class QuestTracker
{
    private readonly DisplayProvider _displayProvider;
    
    private List<QuestStatus> _activeQuests = new();
    private List<QuestStatus> _complateQuests = new();
    
    public IEnumerable<QuestStatus> ActiveQuests => _activeQuests;
    public IEnumerable<QuestStatus> ComplateQuests => _complateQuests;

    public QuestTracker(DisplayProvider displayProvider)
    {
        _displayProvider = displayProvider;
    }
    
    public void AddQuest(QuestConfig quest)
    {
        if (_activeQuests.Exists(q => q.QuestConfig == quest)) 
            return;

        QuestStatus status = new QuestStatus { QuestConfig = quest };
        _activeQuests.Add(status);
        
        _displayProvider.NotificationSystem.AddMessage($"Добавлено задание: {quest.Name}", Palette.HexToColor("#FF9B00"));
    }

    public void ReportProgress(string objectiveId, int amount = 1)
    {
        for (int i = _activeQuests.Count - 1; i >= 0; i--)
        {
            var quest = _activeQuests[i];

            foreach (var objective in quest.QuestConfig.Objectives)
            {
                if (objective.Id == objectiveId)
                {
                    quest.AddProgress(objectiveId, amount);
                    
                    //_notificationSystem.AddMessage($"Обновлен прогресс задания: {objective.Description} ({quest.Progress[objectiveId]}/{objective.RequiredAmount})", Color.white);

                    if (quest.IsCompleted())
                    {
                        _activeQuests.RemoveAt(i);
                        _complateQuests.Add(quest);
                        
                        _displayProvider.NotificationSystem.AddMessage($"Задание завершено: {quest.QuestConfig.Name}", Palette.HexToColor("#FF9B00"));
                    }

                    break;
                }
            }
        }
    }
}