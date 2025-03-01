using R3;
using UnityEngine;

namespace MVVM
{
    public class MessageViewModel : ViewModel
    {
        private readonly ReactiveProperty<string> _text = new();
        private readonly ReactiveProperty<int> _alpha = new();
        
        public Observable<string> Text => _text;
        public Observable<int> Alpha => _alpha;
        
        private readonly MessageModel _messageModel;
        
        private MessageViewModel(MessageModel messageModel)
        {
            _messageModel = messageModel;
        }
        
        public override void Initialize()
        {
            var subscribeText = _messageModel.Text.Subscribe(UpdateText);
            var subscribeAplha = _messageModel.Alpha.Subscribe(UpdateAlpha);
            
            CompositeDisposable.Add(subscribeText);
            CompositeDisposable.Add(subscribeAplha);
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