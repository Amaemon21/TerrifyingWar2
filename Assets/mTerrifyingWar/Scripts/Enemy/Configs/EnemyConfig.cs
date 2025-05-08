using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Enemy/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    [field: SerializeField, BoxGroup("Model"), HorizontalLine] public Enemy EnemyPrefab { get; private set; }
    [field: SerializeField, BoxGroup("EnemyType"), HorizontalLine] public EnemyType EnemyType { get; private set; }
    
    [field: SerializeField, BoxGroup("Patrol Settings"), HorizontalLine] public float PatrolSpeed { get; private set; } = 1f;
    [field: SerializeField, BoxGroup("Patrol Settings")] public float PatrolDuration { get; private set; } = 5f;
    
    [field: SerializeField, BoxGroup("Attack Settings"), HorizontalLine] public float AttackRange { get; private set; } = 2f;
    [field: SerializeField, BoxGroup("Attack Settings")] public float AttackCooldown { get; private set; } = 2f;
    [field: SerializeField, BoxGroup("Attack Settings")] public float SpeedRotation { get; private set; } = 2f;
    [field: SerializeField, BoxGroup("Attack Settings")] public int AttackDamage { get; private set; } = 10;
    [field: SerializeField, BoxGroup("Attack Settings")] public bool IsAttacking { get; private set; }
    
    [field: SerializeField, BoxGroup("Idle Settings"), HorizontalLine] public float IdleDuration { get; private set; } = 5f;
    
    [field: SerializeField, BoxGroup("Chase Settings"), HorizontalLine] public float ChaseSpeed { get; private set; } = 2f;
    
    [field: SerializeField, BoxGroup("Detection Settings"), HorizontalLine] public float DetectionRadius { get; private set; } = 10f;
    [field: SerializeField, BoxGroup("Detection Settings")] public float DetectionAngle { get; private set; } = 90f;

    public void AttackingChanged(bool isAttacking)
    {
        IsAttacking = isAttacking;
    }
}