using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHighlighted : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _highlightedImage;
    
    private void OnEnable()
    {
        _highlightedImage.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _highlightedImage.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _highlightedImage.enabled = false;
    }
}