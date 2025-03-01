using System;
using UnityEngine;

public interface IGameplayFactory : IService
{
    public void CreatePlayer(Vector3 position);
    public void CreateHud();
    public event Action CreateHudChanged;
}