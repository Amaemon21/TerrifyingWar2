using UnityEngine;

public class IdleState : State
{
    public IdleState(Enemy enemy) : base(enemy) { }
    
    private float _elapsedTime;

    public override void Enter()
    {
        Enemy.StopMovement();
        Enemy.EnemyAnimator.Idle(true);
        
        _elapsedTime = 0f;
    }

    public override void Update()
    {
        _elapsedTime += Time.deltaTime;

        if (Enemy.IsInAttackRange())
        {
            Enemy.ChangeState(new AttackState(Enemy));
        }
        
        if (Enemy.DetectPlayer())
        {
            Enemy.ChangeState(new ChaseState(Enemy));
        }
        else if (_elapsedTime >= Enemy.EnemyConfig.IdleDuration)
        {
            Enemy.ChangeState(new PatrolState(Enemy));
        }
    }

    public override void Exit()
    {
        Enemy.EnemyAnimator.Idle(false);
    }
}