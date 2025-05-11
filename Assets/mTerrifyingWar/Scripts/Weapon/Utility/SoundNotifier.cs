using UnityEngine;

public class SoundNotifier : MonoBehaviour
{
    [SerializeField] private Weapon _weapon;
    
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _radiusCast;

    private void OnEnable()
    {
        _weapon.OnShootChanged += NotifyEnemies;
    }

    private void OnDisable()
    {
        _weapon.OnShootChanged -= NotifyEnemies;
    }

    private void NotifyEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radiusCast, _layerMask);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out BodyPart bodyPart))
            {
                if (bodyPart.Enemy.CurrentState is ChaseState || bodyPart.Enemy.CurrentState is DieState)
                    continue;

                bodyPart.Enemy.ChangeState(new ChaseState(bodyPart.Enemy));
            }
        }
    }
    
    public void NotifyEnemies(Vector3 position, float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(position, radius, _layerMask);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out BodyPart bodyPart))
            {
                if (bodyPart.Enemy.CurrentState is ChaseState || bodyPart.Enemy.CurrentState is DieState)
                    continue;

                bodyPart.Enemy.ChangeState(new ChaseState(bodyPart.Enemy));
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radiusCast);
    }
}