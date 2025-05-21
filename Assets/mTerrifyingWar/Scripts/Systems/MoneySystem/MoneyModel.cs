using R3;

public class MoneyModel
{
    private readonly IStorageService _storageService;
    private readonly ReactiveProperty<int> _money = new();
    public Observable<int> Money => _money;
    
    private GameState gameState;
    
    public MoneyModel(IStorageService storageService)
    {
        _storageService = storageService;
        
        //_storageService.Load(LoadSaveData);
    }
    
    public void AddMoney(int amount)
    {
        if (amount < 0)
            return;
        
        _money.Value += amount;
        
        //_saveData.PlayerData.Health = _money.Value;
        //_storageService.Save(_saveData);
    }

    public void RemoveMoney(int amount)
    {
        if (amount < 0)
            return;
        
        _money.Value -= amount;
        
        //_saveData.PlayerData.Health = _money.Value;
        //_storageService.Save(_saveData);
    }

    private void LoadSaveData(GameState gameState)
    {
        this.gameState = gameState;
        
        //MaxHealth = _saveData.PlayerData.MaxHealth;
        //_health.Value = saveData.PlayerData.Health;
    }
}