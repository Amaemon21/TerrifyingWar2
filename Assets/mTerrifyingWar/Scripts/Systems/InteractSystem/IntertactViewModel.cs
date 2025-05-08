using R3;
using UnityEngine;

namespace MVVM
{
    public class IntertactViewModel : ViewModel
    {
        private readonly ReactiveProperty<string> _text = new();
        private readonly ReactiveProperty<Sprite> _icon = new();
        private readonly ReactiveProperty<int> _alpha = new();
    
        public Observable<string> Text => _text;
        public Observable<Sprite> Icon => _icon;
        public Observable<int> Alpha => _alpha;
        
        private readonly InteractModel interactModel;
        
        private IntertactViewModel(InteractModel interactModel)
        {
            this.interactModel = interactModel;
        }
        
        public override void Initialize()
        { 
            var subscribeIcon = interactModel.Icon.Subscribe(UpdateIcon);
            var subscribeText = interactModel.Text.Subscribe(UpdateText);
            var subscribeAplha = interactModel.Alpha.Subscribe(UpdateAlpha);
            
            CompositeDisposable.Add(subscribeIcon);
            CompositeDisposable.Add(subscribeText);
            CompositeDisposable.Add(subscribeAplha);
        }

        private void UpdateIcon(Sprite icon)
        {
            _icon.Value = icon;
        }
        
        private void UpdateText(string text)
        {
            _text.Value = text;
        }
        
        private void UpdateAlpha(bool flag)
        {
            _alpha.Value = flag ? 1 : 0;
        }
    }
}