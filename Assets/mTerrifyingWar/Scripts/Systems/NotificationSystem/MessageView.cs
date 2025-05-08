using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MessageView : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;
    
    [SerializeField] private GameObject _holder;
    [SerializeField] private TMP_Text _messageText;
    
    [SerializeField] private float _durationToShow = 1f;
    [SerializeField] private float _durationToHide = 1f;
    [SerializeField] private float _timeToDestroy = 3f;

    private Vector3 _startPosition;
    
    public void SetMessage(string text, Color color)
    {
        _messageText.text = text;
        _messageText.color = color;

        Setup();
    }

    private void Setup()
    {
        _startPosition = _holder.transform.localPosition;
        
        _holder.transform.DOLocalMove(Vector3.zero, _durationToShow).SetEase(Ease.OutBack).SetLink(_holder.gameObject);
        
        StartCoroutine(DestroyObject());
    }

    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(_timeToDestroy);
    
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_holder.transform.DOLocalMove(_startPosition, _durationToHide).SetEase(Ease.InBack).SetLink(_holder.gameObject));
        sequence.Join(_canvasGroup.DOFade(0f, _durationToHide).SetLink(_canvasGroup.gameObject));
        sequence.OnComplete(() => Destroy(gameObject));
    }

}