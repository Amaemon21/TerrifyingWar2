using UnityEngine;
using Zenject;

public class QuestTriggers : MonoBehaviour
{
    [Inject] private readonly QuestTracker _questTracker;
    
    [SerializeField] private QuestObjectiveConfig _questObjectiveConfig;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMover playerMover))
        {
            _questTracker.ReportProgress(_questObjectiveConfig.Id);
            Destroy(gameObject);
        }
    }
}
