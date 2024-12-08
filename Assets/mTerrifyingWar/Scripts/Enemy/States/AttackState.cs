public class AttackState : State
{
    public AttackState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        Enemy.EnemyAnimator.PlayAttack();
        Enemy.StartAttack();
    }

    public override void Update()
    {
        Enemy.RotateToTarget(Enemy.Target.position);

        if (!Enemy.IsAttacking)
        {
            if (Enemy.IsInAttackRange())
            {
                Enemy.ChangeState(new IdleState(Enemy));
            }
            else
            {
                Enemy.ChangeState(new ChaseState(Enemy));
            }
        }
    }

    public override void Exit() { }
}