using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class QuestView : MonoBehaviour
{
    [Inject] private readonly QuestTracker _questTracker;
    
    [SerializeField, BoxGroup("Content"), HorizontalLine] private Transform _contentQestCell;
    
    [SerializeField, BoxGroup("UI"), HorizontalLine] private QuestCell _questCell;
    [SerializeField, BoxGroup("UI")] private QuestInfo _questInfo;
    
    [SerializeField, BoxGroup("Buttons"), HorizontalLine] private Button _activateQuestButton;
    [SerializeField, BoxGroup("Buttons")] private Button _completedQuestButton;
    
    [SerializeField, BoxGroup("ImageButtons"), HorizontalLine] private Sprite _spriteActiveQuestLine;
    [SerializeField, BoxGroup("ImageButtons")] private Sprite _spriteCompletedQuestLine;
    [SerializeField, BoxGroup("ImageButtons")] private Image _lineImage;
    
    private void OnEnable()
    {
        _activateQuestButton.onClick.AddListener(ShowActiveQuests);
        _completedQuestButton.onClick.AddListener(ShowCompletedQuests);

        ShowActiveQuests();
        
        _questInfo.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _activateQuestButton.onClick.RemoveListener(ShowActiveQuests);
        _completedQuestButton.onClick.RemoveListener(ShowCompletedQuests);
    }
    
    private void ShowActiveQuests()
    {
        _lineImage.sprite = _spriteActiveQuestLine;
        
        SpawnQuestCells(_questTracker.ActiveQuests);
        _questInfo.gameObject.SetActive(false);
    }

    private void ShowCompletedQuests()
    {
        _lineImage.sprite = _spriteCompletedQuestLine;
        
        SpawnQuestCells(_questTracker.ComplateQuests);
        _questInfo.gameObject.SetActive(false);
    }

    private void SpawnQuestCells(IEnumerable<QuestStatus> quests)
    {
        ClearQuestCells();

        foreach (var item in quests)
        {
            QuestCell quest = Instantiate(_questCell, _contentQestCell);
            quest.Setup(this, item);
        }
    }
    
    private void ClearQuestCells()
    {
        foreach (Transform child in _contentQestCell)
        {
            Destroy(child.gameObject);
        }
    }

    public void SetupInfo(QuestStatus questConfig)
    {
        _questInfo.gameObject.SetActive(true);
        _questInfo.Setup(questConfig);
    }
}