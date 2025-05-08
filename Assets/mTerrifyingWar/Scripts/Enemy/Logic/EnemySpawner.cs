using System.Collections;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(UniqueId))]
public class EnemySpawner : MonoBehaviour
{
    [Inject] private readonly DiContainer _container;
    [Inject] private readonly EnemyDatabaseConfig _enemyDatabaseConfig;
    
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private int _maxEnemies;
    [SerializeField] private int _spwanDelay;
    
    private string _id;
    private int _enemiesSpawned;
    private EnemyConfig _enemyConfig;

    private void Awake()
    {
        _id = GetComponent<UniqueId>().Id;
        
        StartCoroutine(StartSpawner());
    }

    private IEnumerator StartSpawner()
    {
        while (_enemiesSpawned < _maxEnemies)
        {
            _enemyConfig = _enemyDatabaseConfig.GetEnemyConfig(enemyType);
            Enemy enemy = _container.InstantiatePrefabForComponent<Enemy>(_enemyConfig.EnemyPrefab, transform.position, Quaternion.identity, null);
            enemy.Setup(_enemyConfig);

            _enemiesSpawned++;
            
            yield return new WaitForSeconds(_spwanDelay);
        }
    }
}