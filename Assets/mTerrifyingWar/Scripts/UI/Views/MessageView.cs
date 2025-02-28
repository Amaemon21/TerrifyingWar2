using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MessageView : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TMP_Text _messageText;
    
    [SerializeField] private float _timeToDestroy = 3f;
    
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = (RectTransform)transform;
    }

    public void SetMessage(string message)
    {
        _messageText.text = message;
        
        _canvasGroup.alpha = 0;
        
        _canvasGroup.DOFade(1f, 0.5f);
        
        StartCoroutine(DestroyObject());
    }

    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(_timeToDestroy);
        
        _canvasGroup.DOFade(0f, 0.3f);
        _rectTransform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack)
            .OnComplete(() => Destroy(gameObject));
    }
}