using UnityEngine;

public class ChaseState : State
{
    private Coroutine _patrolCoroutine;
    private bool _isChase;

    public ChaseState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        Enemy.NavMeshAgent.speed = Enemy.EnemyConfig.ChaseSpeed;
        Enemy.EnemyAnimator.Run(true);
        _isChase = true;
    }

    public override void Update()
    {
        if (Enemy.IsInAttackRange())
        {
            Enemy.ChangeState(new AttackState(Enemy));
        }
        else if (_isChase)
        {
            Enemy.MoveTo(Enemy.Target.transform.position);
        }
    }

    public override void Exit()
    {
        Enemy.EnemyAnimator.Run(false);
        Enemy.StopMovement();
    }
}