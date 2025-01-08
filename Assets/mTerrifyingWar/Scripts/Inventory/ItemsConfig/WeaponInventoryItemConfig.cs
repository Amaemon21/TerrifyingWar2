using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItem", menuName = "Inventory/WeaponItem")]
public class WeaponInventoryItemConfig : InventoryItemConfig
{
    [field: SerializeField, BoxGroup("Weapon item config"), HorizontalLine, ShowAssetPreview] public Sprite EquippedSprite { get; private set;}
    [field: SerializeField, BoxGroup("Weapon item config")] public Weapon WeaponHandPrefab { get; private set;}
    [field: SerializeField, BoxGroup("Weapon item config")] public int Damage { get; private set;}
    [field: SerializeField, BoxGroup("Weapon item config")] public int FireRate { get; private set;}
    
    [field: SerializeField, BoxGroup("Weapon item config")] public string AmmoID { get; private set;}
    [field: SerializeField, BoxGroup("Weapon item config")] public int MagazineSize { get; private set;}
    [field: SerializeField, BoxGroup("Weapon item config")] public int CurrentAmmo { get; private set;}
    
    [field: SerializeField, BoxGroup("Weapon item config"), Range(0, 100)] public int Durability { get; private set;}

    public void AddCurrentAmmo(int value)
    {
        if (value >= 0)
        {
            CurrentAmmo += value;
        }
    }
    
    public void SetCurrentAmmo(int value)
    {
        if (value >= 0)
        {
            CurrentAmmo = value;
        }
    }
    
    public void SetCurrentAmmo()
    {
        CurrentAmmo = MagazineSize;
    }

    public void RemoveCurrentAmmo()
    {
        CurrentAmmo--;
    }

    public void ResetCurrentAmmo()
    {
        CurrentAmmo = 0;
    }
}