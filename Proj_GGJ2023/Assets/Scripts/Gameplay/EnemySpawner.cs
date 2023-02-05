using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{

    public List<EnemyBehaviour> activeEnemies = new List<EnemyBehaviour>();

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
    }


    public void Start()
    {
        currSpawnRate = GetNewSpawnRate();
        activeEnemies.Clear();
    }

    public void Init(EnemiesSpawnerData data)
    {
        spawnRate = data.initialSpawnRate;
        spawnRateRandomness = data.randomness;
        posibleEnemyDatas = data.availableEnemies;
    }

    public void OnGameStart()
    {
        SpawnNewEnemy();        
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
        var pos = GetRandomSpawnPoint();
        newEnemy.transform.position = pos;
        newEnemy.Init(GetRandomEnemyData(), this);
        activeEnemies.Add(newEnemy);
    }

    public void RecycleEnemy(EnemyBehaviour enemyToRecycle)
    {
        activeEnemies.Remove(enemyToRecycle);
        enemiesPool.RecycleItem(enemyToRecycle);
    }

    public EnemyData GetRandomEnemyData()
    {
        var selected = Random.Range(0, posibleEnemyDatas.Length);
        return posibleEnemyDatas[selected];
    }


    public Vector2 GetRandomSpawnPoint()
    {
        string[] positions = new string[] { "left", "right", "top", "bottom" };
        var selected = Random.Range(0, positions.Length - 1);
        string position = positions[selected];
        float x = 0F, y = 0F;
        if (position == "left")
        {
            x = Random.Range(-0.5F, 0F);
            y = Random.Range(0F, 1F);
        }
        else if (position == "right")
        {
            x = Random.Range(1F, 1.5F);
            y = Random.Range(0F, 1F);
        }
        else if (position == "top")
        {
            x = Random.Range(0F, 1F);
            y = Random.Range(1F, 1.5F);
        }
        else if (position == "bottom")
        {
            x = Random.Range(0F, 1F);
            y = Random.Range(-0.5F, 0F);
        }
        Vector3 p1 = Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0F));
        return p1;
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