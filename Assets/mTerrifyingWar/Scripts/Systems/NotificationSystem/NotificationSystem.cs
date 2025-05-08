using UnityEngine;

public class NotificationSystem : MonoBehaviour
{
    [SerializeField] private MessageView _messageViewPrefab;
    [SerializeField] private RectTransform _content;

    public void AddMessage(string text, Color color)
    {
        MessageView newMessage = Instantiate(_messageViewPrefab, _content);
        newMessage.transform.SetSiblingIndex(0);
        newMessage.SetMessage(text, color);
    }
}