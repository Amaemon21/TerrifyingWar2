using System.Linq;
using UnityEngine;
using Zenject;

public class QuestSaver : MonoBehaviour, ISavedProgressReader
{
    [Inject] private readonly QuestTracker _questTracker;
    
    [SerializeField] private QuestConfig _startedQuestConfig;
    
    [SerializeField] private QuestDatabase _questDatabase;
    
    private GameState _gameState;
    
    public void LoadProgress(GameState gameState)
    {
        _gameState = gameState;
        
        _questTracker.OnQuestAdded += AddQuest;
        
        LoadQuestTracker();     
        
        _questTracker.AddQuest(_startedQuestConfig);
    }
    
    private void OnDisable()
    {
        _questTracker.OnQuestAdded -= AddQuest;
    }

    private void LoadQuestTracker()
    {
        foreach (Entity entity in _gameState.Entities)
        {
            if (entity is QuestEntity questEntity)
            {
                QuestConfig questConfig = _questDatabase.FindQuestByID(questEntity.QuestId);
                
                _questTracker.AddQuest(questConfig, true);
            }
        }
    }

    private void AddQuest(QuestConfig questConfig)
    {
        if (questConfig == null)
            return;
        
        if (_gameState.Entities.OfType<QuestEntity>().Any(e => e.QuestId == questConfig.QuestId))
            return;

        QuestEntity questEntity = new QuestEntity
        {
            QuestId = questConfig.QuestId,
            EntityId = _gameState.GetNewId()
        };

        questEntity.QuestObjectiveEntities.AddRange(
            questConfig.Objectives.Select(o => new QuestObjectiveEntity(o.RequiredAmount))
        );

        _gameState.Entities.Add(questEntity);
    }
}