using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State
{
    private float _elapsedTime;
    private SphereCollider _collider;

    public PatrolState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        _collider = Enemy.PatrolCollider;

        Enemy.NavMeshAgent.speed = Enemy.EnemyConfig.PatrolSpeed;
        Enemy.EnemyAnimator.Move(true);
        _elapsedTime = 0f;

        Patrol();
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
            if (Enemy.NavMeshAgent.remainingDistance <= Enemy.NavMeshAgent.stoppingDistance && !Enemy.NavMeshAgent.pathPending)
            {
                Patrol();
            }
        }
    }

    public override void Exit()
    {
        Enemy.EnemyAnimator.Move(false);
        Enemy.StopMovement();
    }

    private void Patrol()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _collider.radius;
        randomDirection += _collider.transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, _collider.radius, NavMesh.AllAreas))
        {
            Enemy.MoveTo(hit.position);
        }
    }
}