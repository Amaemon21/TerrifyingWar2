using UnityEngine;
using UnityEngine.AI;
using Zenject;

[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyAnimator))]
[RequireComponent(typeof(EnemyDeath))]
public class Enemy : MonoBehaviour
{
    [Inject] private readonly IGameplayFactory _gameplayFactory;
    [Inject] private readonly PlayerProvider _playerProvider;
    [Inject] private readonly PlayerHealth _playerHealth;
    
    [SerializeField] private HeadTransform _headTransform;
    [SerializeField] private SphereCollider _patrolCollider;
    
    private Vector3 _positionToLook;
    
    public EnemyConfig EnemyConfig { get; private set; }
    public State CurrentState { get; private set; }
    public NavMeshAgent NavMeshAgent { get; private set; }
    public EnemyAnimator EnemyAnimator { get; private set; }
    public EnemyHealth EnemyHealth { get; private set; }
    public Transform Target { get; private set;}
    public SphereCollider PatrolCollider => _patrolCollider;

    private void Awake()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        EnemyAnimator = GetComponent<EnemyAnimator>();
        EnemyHealth = GetComponent<EnemyHealth>();
    }
    
    private void OnEnable()
    {
        EnemyHealth.EnemyDeath += EnemyDeathChanged;
        _gameplayFactory.CreatePlayerChanged += SetupTarget;
    }

    private void OnDisable()
    {
        EnemyHealth.EnemyDeath -= EnemyDeathChanged;
        _gameplayFactory.CreatePlayerChanged -= SetupTarget;
    }

    private void SetupTarget()
    {
        Target = _playerProvider.PlayerMover.transform;
    }

    public void Setup(EnemyConfig config)
    {
        EnemyConfig = config;
        
        ChangeState(new IdleState(this));
    }

    private void Update()
    {
        CurrentState?.Update();
    }

    public void ChangeState(State newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public bool DetectPlayer()
    {
        if (Target == null)
            return false;
        
        Vector3 enemyPosition = _headTransform.transform.position;
        Vector3 forwardDirection = _headTransform.transform.forward;
        Vector3 directionToPlayer = (Target.transform.position - enemyPosition).normalized;
        
        float distanceToPlayer = Vector3.Distance(enemyPosition, Target.transform.position);

        if (distanceToPlayer < EnemyConfig.DetectionRadius)
        {
            float angle = Vector3.Angle(forwardDirection, directionToPlayer);
            
            if (angle < EnemyConfig.DetectionAngle / 2)
            {
                if (Physics.Raycast(enemyPosition, directionToPlayer, out RaycastHit hit, EnemyConfig.DetectionRadius))
                {
                    if (hit.collider.TryGetComponent(out PlayerMover controller))
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
        if (Target == null)
            return false;
        
        float distanceToPlayer = Vector3.Distance(transform.position, Target.transform.position);
        return distanceToPlayer <= EnemyConfig.AttackRange;
    }

    public void StartAttack()
    {
        EnemyConfig.AttackingChanged(true);
    }

    public void EndAttack()
    {
        EnemyConfig.AttackingChanged(false);
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
        if (IsInAttackRange())
        {
            _playerHealth.TakeDamage(EnemyConfig.AttackDamage);
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

    private float SpeedFactor() => EnemyConfig.SpeedRotation * Time.deltaTime;
    
    private Quaternion TargetRotation(Vector3 position) => Quaternion.LookRotation(position);
}