using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDatabaseConfig", menuName = "Enemy/EnemyDatabaseConfig")]
public class EnemyDatabaseConfig : ScriptableObject
{
    [SerializeField, BoxGroup("EnemyDatabase"), HorizontalLine] private List<EnemyConfig> _enemyDatabase = new();

    public EnemyConfig GetEnemyConfig(EnemyTypeId enemyTypeId)
    {
        return _enemyDatabase.Find(config => config.EnemyTypeId == enemyTypeId);
    } 
}