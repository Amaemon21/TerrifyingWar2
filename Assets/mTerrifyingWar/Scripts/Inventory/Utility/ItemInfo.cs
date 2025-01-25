using R3;

public class ItemInfo
{
    private readonly ReactiveProperty<InventoryItemConfig> _config = new();
    public Observable<InventoryItemConfig> Config => _config;

    public void Setup(InventoryItemConfig config)
    {
        _config.Value = config;
    }
}