using System;
using UnityEngine;

public interface IGameplayFactory : IService
{
    public void CreatePlayer(Transform spawnTransform);
    public void CreateHud();
    public event Action CreateHudChanged;
}