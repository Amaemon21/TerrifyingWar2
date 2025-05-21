using System;
using System.Collections.Generic;
using UnityEngine;

public interface IGameplayFactory : IService
{
    public void CreatePlayer(Transform spawnTransform);
    public void CreateHud();
    public Weapon CreateWeapon(Weapon weaponHandPrefab, Transform parent);
    
    public event Action CreatePlayerChanged;
    public event Action CreateHudChanged;
}