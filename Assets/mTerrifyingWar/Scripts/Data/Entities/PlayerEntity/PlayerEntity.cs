using System;

[Serializable]
public class PlayerEntity
{
    public PositionOnLevel PositionOnLevel { get; set; } = new();
    public HealthEntity HealthEntity { get; set; } = new();
}