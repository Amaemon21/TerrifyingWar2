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
    [Inject] private readonly HeadTransform _headTransform;
    
    [SerializeField, BoxGroup("Enemy Config"), HorizontalLine] private EnemyConfig _enemyConfig;
    
    [field: SerializeField, BoxGroup("Patrol Settings"), HorizontalLine] public Transform[] PatrolPoints { get; private set; }
    
    private EnemyHealth _enemyHealth;
    
    private State _currentState;
    
    private Vector3 _positionToLook;
    
    public NavMeshAgent NavMeshAgent { get; private set; }
    public EnemyAnimator EnemyAnimator{ get; private set; }
    public EnemyConfig EnemyConfig => _enemyConfig;
    public Transform Target => _target.transform;
    public State CurrentState => _currentState;

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
    }

    public bool DetectPlayer()
    {
        Vector3 enemyPosition = _headTransform.transform.position;
        Vector3 forwardDirection = _headTransform.transform.forward;
        Vector3 directionToPlayer = (_target.transform.position - enemyPosition).normalized;
        
        float distanceToPlayer = Vector3.Distance(enemyPosition, _target.transform.position);

        if (distanceToPlayer < _enemyConfig.DetectionRadius)
        {
            float angle = Vector3.Angle(forwardDirection, directionToPlayer);
            
            if (angle < _enemyConfig.DetectionAngle / 2)
            {
                if (Physics.Raycast(enemyPosition, directionToPlayer, out RaycastHit hit, _enemyConfig.DetectionRadius))
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
        return distanceToPlayer <= _enemyConfig.AttackRange;
    }

    public void StartAttack()
    {
        _enemyConfig.AttackingChanged(true);
    }

    public void EndAttack()
    {
        _enemyConfig.AttackingChanged(false);
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
            playerHealth.TakeDamage(_enemyConfig.AttackDamage);
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

    private float SpeedFactor() => _enemyConfig.SpeedRotation * Time.deltaTime;
    
    private Quaternion TargetRotation(Vector3 position) => Quaternion.LookRotation(position);
}