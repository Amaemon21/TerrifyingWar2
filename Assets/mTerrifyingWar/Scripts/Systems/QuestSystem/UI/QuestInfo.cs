using TMPro;
using UnityEngine;

public class QuestInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _authorText;
    [SerializeField] private TMP_Text _questObjectiveText;
    [SerializeField] private TMP_Text _descriptionText;
    
    private QuestStatus _questStatus;
    private QuestConfig _config;

    private void OnDisable()
    {
        gameObject.SetActive(false);
    }

    public void Setup(QuestStatus questStatus)
    {
        _questStatus = questStatus;
        _config = questStatus.QuestConfig;

        _nameText.text = _config.Name;
        _authorText.text = $"Автор: {_config.Author}";

        SetupObjectivesText();
        
        _descriptionText.text = _config.Description;
    }

    private void SetupObjectivesText()
    {
        string objectivesText = "";

        foreach (var item in _config.Objectives)
        {
            int currentProgress = _questStatus.Progress.TryGetValue(item.Id, out var value) ? value : 0;
            bool isCompleted = item.Type != QuestObjectiveType.TriggerOnly
                ? currentProgress >= item.RequiredAmount
                : _questStatus.Progress.ContainsKey(item.Id); // допустим, TriggerOnly просто учитывается как факт

            string line = item.Type != QuestObjectiveType.TriggerOnly
                ? $"- {item.Description} ({currentProgress}/{item.RequiredAmount})"
                : $"- {item.Description}";

            if (isCompleted)
                line = $"<s>{line}</s>";

            objectivesText += line + "\n";
        }

        _questObjectiveText.text = objectivesText;
    }
}