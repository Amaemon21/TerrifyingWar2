using UnityEngine;
using Zenject;

public class CityEntryPoint : GameplayEntryPoint
{
    [Inject] private readonly CityEnterParams _cityEnterParams;
    [Inject] private readonly PlayerProvider _playerProvider;
    
    [SerializeField] private PlayerSpawnExitBonkerPosition _playerSpawnExitBonkerPosition;
    
    public override void Run()
    {
        base.Run();
        
        PersistentProgressService.GameState.PlayerEntity.PositionOnLevel.Level = Scenes.City;

        if (_cityEnterParams.LastScene == Scenes.Bunker)
        {
            _playerProvider.PlayerMover.Warp(_playerSpawnExitBonkerPosition.transform.position);
        }
    }
}