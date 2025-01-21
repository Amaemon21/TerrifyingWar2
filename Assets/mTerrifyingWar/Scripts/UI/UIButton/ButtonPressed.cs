using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPressed : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private readonly float _defaultValue = 1f;
    private readonly float _endValue = 0.9f;
    private readonly float _duration = 0.2f;

    private Transform _transform;
    
    private void Awake()
    {
        _transform = transform;
    }

    private void OnEnable()
    {
        _transform.DOKill();
        
        _transform.localScale = Vector3.one * _defaultValue;
        _transform.DOScale(_defaultValue, _duration);
    }

    private void OnDestroy()
    {
        _transform.DOKill();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_transform == null) 
            return;
        
        _transform.DOKill();
        
        _transform.DOScale(_endValue, _duration);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_transform == null) 
            return;
        
        _transform.DOKill();
        
        _transform.DOScale(_defaultValue, _duration);
    }
}