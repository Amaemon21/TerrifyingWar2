using R3;
using TMPro;
using UnityEngine;
using Zenject;

namespace MVVM
{
    public class MoneyView : View
    {
        [Inject] private readonly MoneyViewModel _playerMoney;
    
        [SerializeField] private TMP_Text _textValue;

        private void OnEnable()
        {
            _playerMoney.Money.Subscribe(OnMoneyChanged).AddTo(CompositeDisposable);
        }

        private void OnMoneyChanged(int value)
        {
            _textValue.text = value.ToString();
        }
    }
}