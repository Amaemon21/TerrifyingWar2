using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NotificationSystem : MonoBehaviour
{
    [SerializeField] private MessageView _messageViewPrefab;
    [SerializeField] private Transform _content;

    private RectTransform _rectTransform;
    
    private void Awake()
    {
        _rectTransform = (RectTransform)_content;
    }

    public void AddMessage(string text)
    {
        MessageView newMessage = Instantiate(_messageViewPrefab, _content);
        newMessage.SetMessage(text);
        
        newMessage.transform.SetSiblingIndex(0);
    
        RectTransform rect = (RectTransform)newMessage.transform;
        rect.localScale = Vector3.zero;

        rect.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);

        LayoutRebuilder.MarkLayoutForRebuild(_rectTransform);
    }

}