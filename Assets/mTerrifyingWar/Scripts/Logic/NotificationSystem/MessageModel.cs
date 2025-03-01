using R3;
using TMPro;
using UnityEngine;
using Zenject;

public class MessageModel
{
    private readonly DiContainer _container;

    private readonly ReactiveProperty<string> _text = new();
    private readonly ReactiveProperty<bool> _alpha = new();
    
    public Observable<string> Text => _text;
    public Observable<bool> Alpha => _alpha;

    public MessageModel(DiContainer container)
    {
        _container = container;
    }

    public MessageView CreateMessage(MessageView messageView, RectTransform content, TMP_Text text)
    {
        MessageView view = _container.InstantiatePrefabForComponent<MessageView>(messageView, content);
        
        return view;
    }
}