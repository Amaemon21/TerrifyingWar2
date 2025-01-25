using MVVM;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ItemInfoView : View
{
    [Inject] private readonly ItemInfoViewModel _viewModel;
    
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _infoText;

    private void OnEnable()
    {
        var subscribeIcon = _viewModel.ItemIcon.Subscribe(UpdateItemIcon);
        var subscribeText = _viewModel.ItemText.Subscribe(UpdateItemText);
        
        CompositeDisposable.Add(subscribeIcon);
        CompositeDisposable.Add(subscribeText);
    }

    private void UpdateItemIcon(Sprite sprite) => _icon.sprite = sprite;

    private void UpdateItemText(string text) => _infoText.text = text;
}