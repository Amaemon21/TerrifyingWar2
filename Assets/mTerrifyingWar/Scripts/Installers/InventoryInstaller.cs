using UnityEngine;
using Zenject;

public class InventoryInstaller : MonoInstaller
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private InventorySystem _inventorySystem;

    public override void InstallBindings()
    {
        Container.Bind<Inventory>().FromInstance(_inventory).AsSingle();
        Container.Bind<InventorySystem>().FromInstance(_inventorySystem).AsSingle();
    }
}