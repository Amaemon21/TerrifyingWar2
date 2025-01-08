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
        transform.DOScale(_defaultValue, _duration);
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(_endValue, _duration);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(_defaultValue, _duration);
    }
}