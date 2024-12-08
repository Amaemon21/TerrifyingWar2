using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [SerializeField] private RagdollHandler _ragdollHandler;
    [SerializeField] private EnemyHealth _enemyHealth;

    private void OnValidate()
    {
        _ragdollHandler ??= GetComponent<RagdollHandler>();
        _enemyHealth ??= GetComponent<EnemyHealth>();
    }

    public void Hit(Vector3 force, Vector3 hitPosition)
    {
        _ragdollHandler.Hit(force, hitPosition);
    }

    public void TakeDamage(int damage)
    {
        _enemyHealth.TakeDamage(damage);
    }
}
