using System;

[Serializable]
public class HealthEntity
{
    public float CurrentHealth;
    public float MaxHealth;

    public void ResetHealth()
    {
        CurrentHealth = MaxHealth;
    } 
}