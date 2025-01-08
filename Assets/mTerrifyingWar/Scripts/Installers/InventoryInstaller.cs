using UnityEngine;
using Zenject;

public class InventoryInstaller : MonoInstaller
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private InventoryManager _inventoryManager;

    public override void InstallBindings()
    {
        Container.Bind<Inventory>().FromInstance(_inventory).AsSingle();
        Container.Bind<InventoryManager>().FromInstance(_inventoryManager).AsSingle();
    }
}