using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesSpawnerData", menuName = "GGJ/New Enemies Spawner Data")]
public class EnemiesSpawnerData : ScriptableObject
{
    public float initialSpawnRate = 5;
    public float randomness = 0.5f;
    public EnemyData[] availableEnemies;
}
