using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Quest", menuName = "Quests/Quest Database")]
public class QuestDatabase : ScriptableObject
{
    [field: SerializeField, Expandable] public List<QuestConfig> QuestConfigs;
    
    public QuestConfig FindQuestByID(string id)
    {
        if (string.IsNullOrEmpty(id))
            return null;

        QuestConfig item = QuestConfigs.FirstOrDefault(config => config.QuestId == id);

        if (item != null)
            return item;
        
        return null;
    }
}