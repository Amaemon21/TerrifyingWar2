using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [SerializeField] private RagdollHandler _ragdollHandler;
    [SerializeField] private EnemyHealth _enemyHealth;
    [SerializeField] private Enemy _enemy;

    private void OnValidate()
    {
        _ragdollHandler ??= GetComponentInParent<RagdollHandler>();
        _enemyHealth ??= GetComponentInParent<EnemyHealth>();
        _enemy ??= GetComponentInParent<Enemy>();
    }

    public void Hit(Vector3 force, Vector3 hitPosition)
    {
        _ragdollHandler.Hit(force, hitPosition);
    }

    public void TakeDamage(int damage)
    {
        _enemyHealth.TakeDamage(damage);

        if (_enemy.CurrentState is ChaseState)
            return;

        if (_enemy.CurrentState is DieState)
            return;

        _enemy.ChangeState(new ChaseState(_enemy));
    }
}