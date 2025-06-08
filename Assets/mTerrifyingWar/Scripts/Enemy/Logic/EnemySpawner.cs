using System.Collections;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(UniqueId))]
public class EnemySpawner : MonoBehaviour
{
    [Inject] private readonly IGameplayFactory _gameplayFactory;
    [Inject] private readonly DiContainer _container;
    
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private int _maxEnemies;
    [SerializeField] private int _spwanDelay;
    
    private string _id;
    private int _enemiesSpawned;
    private EnemyConfig _enemyConfig;
    private EnemyDatabaseConfig _enemyDatabaseConfig;

    private void OnEnable()
    {
        _gameplayFactory.CreatePlayerChanged += Spawn;
    }

    private void OnDisable()
    {
        _gameplayFactory.CreatePlayerChanged -= Spawn;
    }

    private void Spawn()
    {
        _enemyDatabaseConfig = Resources.Load<EnemyDatabaseConfig>(AssetsPath.EnemyDatabasePath);
        
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