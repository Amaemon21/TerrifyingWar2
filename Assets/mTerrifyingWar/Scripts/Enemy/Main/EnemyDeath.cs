using UnityEngine;
using Zenject;

public class EnemyDeath : MonoBehaviour
{
    [Inject] private readonly QuestEvents _questEvents;
    
    [SerializeField] private QuestObjectiveConfig _questObjectiveConfig;
    
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
            _questEvents.Report(_questObjectiveConfig.Id);
            _isDeath = true;
        }
    }
}
