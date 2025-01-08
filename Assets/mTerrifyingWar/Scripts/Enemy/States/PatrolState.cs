using UnityEngine;

public class PatrolState : State
{
    private float _elapsedTime;
    private int _currentPatrolIndex;
    
    public PatrolState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        Enemy.NavMeshAgent.speed = Enemy.EnemyConfig.PatrolSpeed;
        Enemy.EnemyAnimator.Move(true);
        Patrol();
        _elapsedTime = 0f;
    }

    public override void Update()
    {
        _elapsedTime += Time.deltaTime;

        if (Enemy.DetectPlayer())
        {
            Enemy.ChangeState(new ChaseState(Enemy));
        }
        else if (_elapsedTime >= Enemy.EnemyConfig.PatrolDuration)
        {
            Enemy.ChangeState(new IdleState(Enemy));
        }
        else
        {
            Patrol();
        }
    }

    public override void Exit()
    {
        Enemy.EnemyAnimator.Move(false);
        Enemy.StopMovement();
    }
    
    private void Patrol()
    {
        if (Enemy.PatrolPoints.Length == 0) 
            return;

        if (Enemy.NavMeshAgent.remainingDistance <= Enemy.NavMeshAgent.stoppingDistance && !Enemy.NavMeshAgent.pathPending)
        {
            _currentPatrolIndex = (_currentPatrolIndex + 1) % Enemy.PatrolPoints.Length;

            _currentPatrolIndex = Random.Range(0, Enemy.PatrolPoints.Length);
            
            Enemy.MoveTo(Enemy.PatrolPoints[_currentPatrolIndex].position);
        }
    }
}