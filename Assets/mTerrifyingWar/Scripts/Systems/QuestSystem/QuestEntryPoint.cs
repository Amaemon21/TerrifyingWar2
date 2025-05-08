using UnityEngine;
using Zenject;

public class QuestEntryPoint : MonoBehaviour
{
    [Inject] private readonly IGameplayFactory _gameplayFactory;
    [Inject] private readonly QuestTracker _questTracker;
    
    [SerializeField] private QuestConfig _startedQuestConfig;
    
    private void OnEnable()
    {
        _gameplayFactory.CreateHudChanged += Setup;
    }

    private void OnDisable()
    {
        _gameplayFactory.CreateHudChanged -= Setup;
    }

    private void Setup()
    {
        _questTracker.AddQuest(_startedQuestConfig);
    }
}