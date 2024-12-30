using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Enemy enemyPrefab;
    [Space]
    [SerializeField] private float spawnRate;

    private Pool<Enemy> enemyPool;
    private Coroutine spawnCoroutine;

    private void Start()
    {
        InitPool();


        spawnCoroutine = StartCoroutine(Spawning());
    }

    [ContextMenu("Spawn")]
    public void Spawn(Vector3 posOffset)
    {
        Enemy enemy = enemyPool.Get();
        enemy.InitWithPool(enemyPool);
        enemy.transform.position = spawnPoint.position + posOffset;

    }

    private IEnumerator Spawning()
    {
        float delayBetweenSpawns = 60f / spawnRate;
        while (true)
        {
            yield return new WaitForSeconds(delayBetweenSpawns + GetRandomOffset());

            Spawn(GetRandomPosOffset());
        }
    }

    private float GetRandomOffset()
    {
        float rateRandomness = 0.2f;

        return Random.Range(-rateRandomness, rateRandomness);
    }

    private Vector3 GetRandomPosOffset()
    {
        float posRandomness = 7f;

        return new Vector3(Random.Range(-posRandomness, posRandomness), 0f, 0f);
    }

    private void InitPool()
    {
        int initPoolCapacity = 30;
        enemyPool = Pool.Create(enemyPrefab, 0, transform);

        for (int i = 0; i < initPoolCapacity; i++)
        {
            enemyPool.Take(Instantiate(enemyPrefab));
        }
    }
}
