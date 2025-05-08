using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestCell : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private Image _image;
    
    private QuestView _questView;
    private QuestStatus _questStatus;
    private QuestConfig _questConfig;

    public void Setup(QuestView questView, QuestStatus questStatus)
    {
        _questStatus = questStatus;
        _questConfig = _questStatus.QuestConfig;
        _image.sprite = _questConfig.Sprite;
        _nameText.text = _questConfig.Name;
        _questView = questView;
    }

    public void SetupInfo()
    {
        _questView.SetupInfo(_questStatus);
    }
}
