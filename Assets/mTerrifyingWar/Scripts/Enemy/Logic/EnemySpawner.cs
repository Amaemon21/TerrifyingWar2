using UnityEngine;
using Zenject;

[RequireComponent(typeof(UniqueId))]
public class EnemySpawner : MonoBehaviour
{
    [Inject] private readonly DiContainer _container;
    [Inject] private readonly EnemyDatabaseConfig _enemyDatabaseConfig;
    
    [SerializeField] private EnemyTypeId _enemyTypeId;
    
    private string _id;
    private EnemyConfig _enemyConfig;

    private void Awake()
    {
        _id = GetComponent<UniqueId>().Id;
        
        StartSpawner();
    }

    private void StartSpawner()
    {
        _enemyConfig = _enemyDatabaseConfig.GetEnemyConfig(_enemyTypeId);
        
        Enemy enemy = _container.InstantiatePrefabForComponent<Enemy>(_enemyConfig.EnemyPrefab, transform.position, Quaternion.identity, null);
        
        enemy.Setup(_enemyConfig);
    }
}