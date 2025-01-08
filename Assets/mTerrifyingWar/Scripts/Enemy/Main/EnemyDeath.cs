using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    [SerializeField] private EnemyHealth _enemyHealth;
    [SerializeField] private RagdollHandler _ragdollHandler;
    
    private bool _isDeath;

    private void OnValidate()
    {
        _enemyHealth ??= GetComponent<EnemyHealth>();
        _ragdollHandler ??= GetComponent<RagdollHandler>();
    }
    
    private void OnEnable()
    {
        _enemyHealth.EnemyDeath += EnemyDeathChanged;
    }

    private void OnDestroy()
    {
        _enemyHealth.EnemyDeath -= EnemyDeathChanged;
    }

    private void EnemyDeathChanged()
    {
        if (_isDeath == false)
        {
            _ragdollHandler.Enable();

            _isDeath = true;
        }
    }
}
