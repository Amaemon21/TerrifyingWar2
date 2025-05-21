using DG.Tweening;
using UnityEngine;

public class DropArea : MonoBehaviour
{
    [SerializeField] private float _hideY;
    [SerializeField] private float _showY;
    [SerializeField] private float _duration;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        Hide(true);
    }

    public void Show()
    {
        _rectTransform.DOAnchorPosY(_showY, _duration).SetLink(_rectTransform.gameObject);
    }

    public void Hide(bool immediate = false)
    {
        if (immediate)
            _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, _hideY);
        else
            _rectTransform.DOAnchorPosY(_hideY, _duration).SetLink(_rectTransform.gameObject);;
    }
}