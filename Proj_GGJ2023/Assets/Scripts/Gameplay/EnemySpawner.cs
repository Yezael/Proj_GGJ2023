using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public EnemyBehaviour enemyPrefab;

    private float timer;
    private float currSpawnRate;
    private float spawnRate = 1;
    private float spawnRateRandomness = 1;
    private Pool<EnemyBehaviour> enemiesPool;
    private EnemyData[] posibleEnemyDatas;

    public void Awake()
    {
        timer = 0;
        enemiesPool = new Pool<EnemyBehaviour>(transform,enemyPrefab, 30);
        currSpawnRate = GetNewSpawnRate();
    }

    public void Init(EnemiesSpawnerData data)
    {
        spawnRate = data.initialSpawnRate;
        spawnRateRandomness = data.randomness;
        posibleEnemyDatas = data.availableEnemies;

    }

    private void Update()
    {
        if (GameManager.Instance.gameState != GameState.Playing) return;
        timer += Time.deltaTime;
        if(timer >= currSpawnRate)
        {
            SpawnNewEnemy();
            timer = 0;
        }
    }


    public float GetNewSpawnRate()
    {
        return (spawnRate + Random.Range(-spawnRateRandomness, spawnRateRandomness)) * GameManager.Instance.GetCurrProgressMultiplier();
    }

    public void SpawnNewEnemy()
    {
        var newEnemy = enemiesPool.GetItem();
        newEnemy.Init(GetRandomEnemyData(), enemiesPool);
    }

    public EnemyData GetRandomEnemyData()
    {
        var selected = Random.Range(0, posibleEnemyDatas.Length);
        return posibleEnemyDatas[selected];
    }

}

[Serializable]
public class Pool<T> where T : Component
{
    private Transform itemsParent;
    private T originalPrefab;
    List<T> availableObjs;
    public Pool(Transform _itemsParent ,T prefab, int initialSize)
    {
        originalPrefab = prefab;
        availableObjs = new List<T>();
        itemsParent = _itemsParent;
        Grow(initialSize);
    }
    private void Grow(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var newObj = GameObject.Instantiate(originalPrefab);
            newObj.gameObject.SetActive(false);
            newObj.transform.SetParent(itemsParent);
            availableObjs.Add(newObj);
        }
    }
    public T GetItem()
    {
        if(availableObjs.Count == 0)
        {
            Grow(10); //Growing by ten as default...Just because.
        }
        var availableObj = availableObjs[0];
        availableObj.gameObject.SetActive(true);
        availableObjs.RemoveAt(0);
        return availableObj;
    }

    public void RecycleItem(T objToRecycle)
    {
        objToRecycle.gameObject.SetActive(false);
        availableObjs.Add(objToRecycle);
    }
}