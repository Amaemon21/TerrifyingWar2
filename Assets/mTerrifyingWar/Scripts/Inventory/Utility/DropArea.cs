using System;
using DG.Tweening;
using UnityEngine;

public class DropArea : MonoBehaviour
{
    [SerializeField] private float _hideValue;
    [SerializeField] private float _showValue;
    
    [SerializeField] private float _duration;

    private RectTransform _rectTransform;
    
    private void Awake()
    {
        _rectTransform = (RectTransform)transform;
    }

    private void OnEnable()
    {
        Hide();
    }

    public void Show()
    {
        _rectTransform.DOMoveY(_showValue, _duration);
    }

    public void Hide()
    {
        _rectTransform.DOMoveY(_hideValue, _duration);
    }
}