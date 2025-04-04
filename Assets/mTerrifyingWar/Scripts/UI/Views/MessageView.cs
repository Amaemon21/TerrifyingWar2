using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MessageView : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TMP_Text _messageText;
    
    [SerializeField] private float _timeToDestroy = 3f;

    public void SetMessage(string text, Color color)
    {
        _messageText.text = text;
        _messageText.color = color;

        Setup();
        
        StartCoroutine(DestroyObject());
    }

    private void Setup()
    {
        _rectTransform.localScale = Vector3.zero;
        _canvasGroup.DOFade(1f, 0.3f).SetLink(_canvasGroup.gameObject);
        _rectTransform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack).SetLink(_rectTransform.gameObject);
    }

    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(_timeToDestroy);
        
        _canvasGroup.DOFade(0f, 0.3f).SetLink(_canvasGroup.gameObject);
        _rectTransform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() => Destroy(gameObject)).SetLink(_rectTransform.gameObject);
    }
}