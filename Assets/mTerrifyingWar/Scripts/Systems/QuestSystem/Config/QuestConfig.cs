using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quests/New Quest")]
public class QuestConfig : ScriptableObject
{
    [field: SerializeField, BoxGroup("Quest config"), HorizontalLine] public string QuestId { get; private set; }
    [field: SerializeField, BoxGroup("Quest config")] public string Name { get; private set; }
    [field: SerializeField, BoxGroup("Quest config")] public string Author { get; private set; }
    [field: SerializeField, BoxGroup("Quest config"), TextArea] public string Description { get; private set; }
    [field: SerializeField, BoxGroup("Quest config"), ShowAssetPreview] public Sprite Sprite { get; private set; }
    [field: SerializeField, Space(10), BoxGroup("Quest config"), Expandable] public QuestObjectiveConfig[] Objectives { get; private set; }
}