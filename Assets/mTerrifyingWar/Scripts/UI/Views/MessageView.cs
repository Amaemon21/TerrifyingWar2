using System;
using System.Collections;
using DG.Tweening;
using MVVM;
using TMPro;
using UnityEngine;
using Zenject;

public class MessageView : View
{
    [Inject] private readonly MessageViewModel _messageViewModel;
    
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TMP_Text _messageText;
    
    [SerializeField] private float _timeToDestroy = 3f;
    
    private RectTransform _rectTransform;
    
    private void Awake()
    {
        _rectTransform = (RectTransform)transform;
        
        _canvasGroup.alpha = 0;
    }
    
    private void OnEnable()
    {
        IDisposable subscribeText = _messageViewModel.Text.Subscribe(UpdateText);
        IDisposable subscribeAlpha = _messageViewModel.Alpha.Subscribe(UpdateAlpha);
            
        CompositeDisposable.Add(subscribeText);
        CompositeDisposable.Add(subscribeAlpha);
    }
    
    private void UpdateText(string text)
    {
        _messageText.text = text;
        
        StartCoroutine(DestroyObject());
    }
        
    private void UpdateAlpha(int alpha)
    {
        _canvasGroup.DOFade(alpha, 0.25f);
    }

    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(_timeToDestroy);

        if (_canvasGroup != null && _canvasGroup.alpha > 0)
        {
            _canvasGroup.DOFade(0f, 0.3f);
        }

        _rectTransform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack)
            .OnComplete(() => Destroy(gameObject));
    }
}