using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DOButtonHighlighted : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _lefLine;
    [SerializeField] private Image _rigthLine;
    [SerializeField] private float _fillDuration = 0.6f;

    private void Awake()
    {
        _lefLine.fillAmount = 0f;
        _rigthLine.fillAmount = 0f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _lefLine.DOFillAmount(1, _fillDuration).SetLink(_lefLine.gameObject);
        _rigthLine.DOFillAmount(1, _fillDuration).SetLink(_rigthLine.gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _lefLine.DOFillAmount(0, _fillDuration).SetLink(_lefLine.gameObject);
        _rigthLine.DOFillAmount(0, _fillDuration).SetLink(_rigthLine.gameObject);
    }
}