public class BunkerEntryPoint : GameplayEntryPoint
{
    public override void Run()
    {
        base.Run();
        
        PersistentProgressService.GameState.PlayerEntity.PositionOnLevel.Level = Scenes.Bunker;
    }
}