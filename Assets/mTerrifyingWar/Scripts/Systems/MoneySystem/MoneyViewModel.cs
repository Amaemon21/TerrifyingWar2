using MVVM;
using R3;

public class MoneyViewModel : ViewModel
{
    private readonly MoneyModel moneyModel;
    private readonly ReactiveProperty<int> _money = new();
    
    public Observable<int> Money => _money;

    public MoneyViewModel(MoneyModel moneyModel)
    {
        this.moneyModel = moneyModel;
    }
    
    public override void Initialize()
    {
        moneyModel.Money.Subscribe(OnMoneyChanged).AddTo(CompositeDisposable);
    }

    private void OnMoneyChanged(int amount)
    {
        _money.Value = amount;
    }
}