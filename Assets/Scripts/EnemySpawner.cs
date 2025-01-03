using Redcode.Pools;
using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Car playerCar;
    [SerializeField] private float spawnDistanceOffset;
    [Space]
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private float spawnRate;

    private Pool<Enemy> enemyPool;
    private Coroutine spawnCoroutine;

    private void Start()
    {
        InitPool();
        spawnCoroutine = StartCoroutine(Spawning());
    }

    [ContextMenu("Spawn")]
    public void Spawn(Vector3 pos)
    {
        Enemy enemy = enemyPool.Get();
        enemy.transform.position = pos;
        enemy.transform.rotation = Quaternion.AngleAxis(180f, Vector3.up);
        enemy.Init(enemyPool, playerCar);
    }

    private IEnumerator Spawning()
    {
        float delayBetweenSpawns = 60f / spawnRate;
        while (true)
        {
            yield return new WaitForSeconds(delayBetweenSpawns + GetRandomTimeOffset());

            Spawn(playerCar.transform.position + GetRandomPosOffset());
        }
    }

    private float GetRandomTimeOffset()
    {
        float rateRandomness = 0.2f;

        return Random.Range(-rateRandomness, rateRandomness);
    }

    private Vector3 GetRandomPosOffset()
    {
        float xPosRandomness = 7f;

        return new Vector3(Random.Range(-xPosRandomness, xPosRandomness), 0f, spawnDistanceOffset);
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
