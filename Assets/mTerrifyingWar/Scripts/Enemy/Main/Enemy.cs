using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyAnimator))]
[RequireComponent(typeof(EnemyDeath))]
public class Enemy : MonoBehaviour
{
    [Inject] private readonly PlayerController _target;

    [SerializeField, BoxGroup("Patrol Settings"), HorizontalLine] private Transform[] _patrolPoints;
    [SerializeField, BoxGroup("Patrol Settings")] private float _patrolSpeed = 1f;
    [SerializeField, BoxGroup("Patrol Settings")] private float _patrolDuration = 5f;
    
    [SerializeField, BoxGroup("Attack Settings"), HorizontalLine] private float _attackRange = 2f;
    [SerializeField, BoxGroup("Attack Settings")] private float _attackCooldown = 2f;
    [SerializeField, BoxGroup("Attack Settings")] private float _speedRotation = 2f;
    [SerializeField, BoxGroup("Attack Settings")] private int _attackDamage = 10;
    [SerializeField, BoxGroup("Attack Settings")] private bool _isAttacking;
    
    [SerializeField, BoxGroup("Idle Settings"), HorizontalLine] private float _idleDuration = 5f;
    
    [SerializeField, BoxGroup("Chase Settings"), HorizontalLine] private float _timeToChase  = 5f;
    [SerializeField, BoxGroup("Chase Settings")] private float _chaseSpeed = 2f;
    
    [SerializeField, BoxGroup("Detection Settings"), HorizontalLine] private float _detectionRadius = 10f;
    [SerializeField, BoxGroup("Detection Settings")] private float _detectionAngle = 90f;
    [SerializeField, BoxGroup("Detection Settings")] private Transform _headTransform;

    private EnemyHealth _enemyHealth;
    
    private State _currentState;
    
    private Vector3 _positionToLook;
    
    public NavMeshAgent NavMeshAgent { get; private set; }
    public EnemyAnimator EnemyAnimator{ get; private set; }
    public Transform Target => _target.transform;
    public float IdleDuration => _idleDuration;
    public float PatrolDuration => _patrolDuration;
    public float TimeToChase => _timeToChase;
    public float PatrolSpeed => _patrolSpeed;
    public float ChaseSpeed => _chaseSpeed;
    public bool IsAttacking => _isAttacking;

    public Transform[] PatrolPoints => _patrolPoints;

    private void Awake()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        EnemyAnimator = GetComponent<EnemyAnimator>();
        _enemyHealth = GetComponent<EnemyHealth>();
    }

    private void Start()
    {
        ChangeState(new IdleState(this));
    }

    private void OnEnable()
    {
        _enemyHealth.EnemyDeath += EnemyDeathChanged;
    }

    private void OnDestroy()
    {
        _enemyHealth.EnemyDeath -= EnemyDeathChanged;
    }

    private void Update()
    {
        _currentState?.Update();
    }

    public void ChangeState(State newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
        Debug.Log(newState);
    }

    public bool DetectPlayer()
    {
        Vector3 enemyPosition = _headTransform.position;
        Vector3 forwardDirection = _headTransform.forward;
        Vector3 directionToPlayer = (_target.transform.position - enemyPosition).normalized;
        
        float distanceToPlayer = Vector3.Distance(enemyPosition, _target.transform.position);

        if (distanceToPlayer < _detectionRadius)
        {
            float angle = Vector3.Angle(forwardDirection, directionToPlayer);
            
            if (angle < _detectionAngle / 2)
            {
                if (Physics.Raycast(enemyPosition, directionToPlayer, out RaycastHit hit, _detectionRadius))
                {
                    if (hit.collider.TryGetComponent(out PlayerController controller))
                    {
                        return true;
                    }
                }
            }
        }
        
        return false;
    }

    public bool IsInAttackRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _target.transform.position);
        return distanceToPlayer <= _attackRange;
    }

    public void StartAttack()
    {
        _isAttacking = true;
    }

    public void EndAttack()
    {
        _isAttacking = false;
    }

    public void MoveTo(Vector3 position)
    {
        if (NavMeshAgent.isActiveAndEnabled)
        {
            RotateToTarget(position);
            NavMeshAgent.SetDestination(position);
        }
    }

    public void StopMovement()
    {
        if (NavMeshAgent.isActiveAndEnabled)
        {
            NavMeshAgent.ResetPath();
        }
    }

    public void DealDamage()
    {
        if (IsInAttackRange() && Target.TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            playerHealth.TakeDamage(_attackDamage);
        }
    }
    
    private void EnemyDeathChanged()
    {
        ChangeState(new DieState(this));
    }

    public void RotateToTarget(Vector3 target)
    {
        UpdatePositionToLookAt(target);

        transform.rotation = SmoothedRotation(transform.rotation, _positionToLook);
    }
    
    private void UpdatePositionToLookAt(Vector3 target)
    {
        Vector3 positionDelta = target - transform.position;
        _positionToLook = new Vector3(positionDelta.x, transform.position.y, positionDelta.z);
    }
    
    private Quaternion SmoothedRotation(Quaternion rotation, Vector3 positionToLook) => Quaternion.Lerp(rotation, TargetRotation(positionToLook), SpeedFactor());

    private float SpeedFactor() => _speedRotation * Time.deltaTime;
    
    private Quaternion TargetRotation(Vector3 position) => Quaternion.LookRotation(position);
}