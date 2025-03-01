using UnityEngine;
using DG.Tweening;
using Zenject;

public class NotificationSystem : MonoBehaviour
{
    [Inject] private readonly MessageModel _messageModel;
    
    [SerializeField] private MessageView _messageViewPrefab;
    [SerializeField] private RectTransform _content;

    public void AddMessage(string text)
    {
        MessageView newMessage = _messageModel.CreateMessage(_messageViewPrefab, _content);

        newMessage.transform.SetSiblingIndex(0);
    
        RectTransform rect = (RectTransform)newMessage.transform;
        rect.localScale = Vector3.zero;

        rect.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
    }
}