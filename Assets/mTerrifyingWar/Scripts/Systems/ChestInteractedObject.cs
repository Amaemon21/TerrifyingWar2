using UnityEngine;
using Zenject;

public class ChestInteractedObject : InteractObject
{
    [Inject] private readonly DisplayProvider _displayProvider;
    [Inject] private readonly UIWindowService _windowService;
    
    [SerializeField] private int _minItems;
    [SerializeField] private int _maxItems;
    
    private LootGenerator _lootGenerator;
    
    protected override void OnInteract()
    {
        _lootGenerator = new LootGenerator(_minItems, _maxItems);
        
        _windowService.OpenWindow(WindowType.Inventory);
        
        _displayProvider.InventoryComponent.LootSlots.Setup(_lootGenerator.GenerateLoot());
    }
}