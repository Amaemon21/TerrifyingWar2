using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Objective ID", fileName = "Objective")]
public class QuestObjectiveConfig : ScriptableObject
{
    [field: SerializeField, Space(10), BoxGroup("Quest Objective"), HorizontalLine] public string Id { get; private set; }
    [field: SerializeField, Space(10), BoxGroup("Quest Objective")] public QuestObjectiveType Type { get; private set; } = QuestObjectiveType.ProgressAmount;
    
    [field: SerializeField, Space(10), BoxGroup("Quest Objective")] public string Description { get; private set; }
    
    [SerializeField, Space(10), BoxGroup("Quest Objective"), ShowIf(nameof(HasProgressAmount))] private int _requiredAmount = 1;
    
    public int RequiredAmount => _requiredAmount;
    
    private bool HasProgressAmount()
    {
        return Type == QuestObjectiveType.ProgressAmount;
    }
}
