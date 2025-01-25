using System;
using DG.Tweening;
using R3;
using TMPro;
using Zenject;
using UnityEngine;
using UnityEngine.UI;

namespace MVVM
{
    public class InteractView : View
    {
        [Inject] private IntertactViewModel _intertactViewModel;
        
        [SerializeField] private TMP_Text _interactText;
        [SerializeField] private Image _interactImage;
        [SerializeField] private CanvasGroup _canvasGroup;
        
        private void OnEnable()
        {
            IDisposable subscribeText = _intertactViewModel.Text.Subscribe(UpdateText);
            IDisposable subscribeIcon = _intertactViewModel.Icon.Subscribe(UpdateIcon);
            IDisposable subscribeAlpha = _intertactViewModel.Alpha.Subscribe(UpdateAlpha);
            
            CompositeDisposable.Add(subscribeText);
            CompositeDisposable.Add(subscribeIcon);
            CompositeDisposable.Add(subscribeAlpha);
        }
        
        private void UpdateIcon(Sprite icon)
        {
            _interactImage.sprite = icon;
        }
        
        private void UpdateText(string text)
        {
            _interactText.text = text;
        }
        
        private void UpdateAlpha(int alpha)
        {
            _canvasGroup.DOFade(alpha, 0.25f);
        }
    }
}