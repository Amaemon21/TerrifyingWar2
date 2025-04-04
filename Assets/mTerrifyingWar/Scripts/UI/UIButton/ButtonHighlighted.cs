using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHighlighted : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private readonly float _duration = 0.2f;
    
    private readonly float _targetScale = 1.1f;
    
    private Vector3 _defaultLocaScale;
    
    private void OnEnable()
    {
        _defaultLocaScale = transform.localScale;
    }

    private void OnDisable()
    {
        transform.localScale = _defaultLocaScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(_targetScale, _duration).SetLink(transform.gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(_defaultLocaScale, _duration).SetLink(transform.gameObject);
    }
}