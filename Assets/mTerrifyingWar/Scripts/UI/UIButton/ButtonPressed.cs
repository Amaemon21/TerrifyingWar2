using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPressed : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private readonly float _defaultValue = 1f;
    private readonly float _endValue = 0.9f;
    private readonly float _duration = 0.2f;

    private void OnEnable()
    {
        transform.localScale = Vector3.one * _defaultValue;
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.one * _defaultValue;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(_endValue, _duration).SetLink(transform.gameObject);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(_defaultValue, _duration).SetLink(transform.gameObject);
    }
}