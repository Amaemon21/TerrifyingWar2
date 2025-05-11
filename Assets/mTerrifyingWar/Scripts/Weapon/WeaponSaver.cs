using System.Collections;
using UnityEngine;
using Zenject;

public class WeaponSaver : MonoBehaviour
{
    [Inject] private readonly IStorageService _storageService;

    [SerializeField] private Weapon _weapon;
    
    private GameState _gameState;

    public WeaponItemEntity WeaponItemEntity { get; private set; }

    private void OnEnable()
    {
        _weapon.OnShootChanged += Shoot;
    }

    private void OnDisable()
    {
        _weapon.OnShootChanged -= Shoot;
    }

    public void Initialize()
    {
        _storageService.Load(LoadData);
    }
    
    private void LoadData(GameState gameState)
    {
        _gameState = gameState;

        foreach (var entity in _gameState.Entities)
        {
            if (entity is WeaponItemEntity weaponItemEntity)
            {
                if (weaponItemEntity.EntityId == _weapon.WeaponInventoryItemConfig.SaveId)
                {
                    if (weaponItemEntity.ItemId == _weapon.WeaponInventoryItemConfig.ItemID)
                    {
                        WeaponItemEntity = weaponItemEntity;
                    }
                }
            }
        }
    }

    private void Shoot()
    {
        if (WeaponItemEntity == null)
            return;
        
        WeaponItemEntity.CurrentAmmo--;

        StartCoroutine(SaveData());
    }

    private IEnumerator SaveData()
    {
        yield return null;
        _storageService.Save(_gameState);
    }
}