using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IPoolObject
{
    [SerializeField] private float maxHealth;

    private Pool<Enemy> pool;
    private Coroutine delayedReturn;

    private float health;


    public void OnCreatedInPool(){}

    public void OnGettingFromPool()
    {
        health = maxHealth;
        CancelDelayedReturn();
    }

    public void InitWithPool(Pool<Enemy> newPool)
    {
        pool = newPool;
        ReturningToPoolAfterDelay();
    }

    public void ReturningToPoolAfterDelay()
    {
        CancelDelayedReturn();
        delayedReturn = StartCoroutine(ReturningToPool());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Bullet bullet))
        {
            ApplyDamage(bullet.Damage);
        }
    }

    private void ApplyDamage(float damage)
    {
        health = Mathf.Clamp(health - damage, 0f, maxHealth);
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("die");
        pool.Take(this);
    }

    private void CancelDelayedReturn()
    {
        if (delayedReturn != null)
            StopCoroutine(delayedReturn);
    }

    private IEnumerator ReturningToPool()
    {
        yield return new WaitForSeconds(10f);

        if (pool == null)
        {
            Debug.LogError("no pool in enemy");
        }
        else
        {
            pool.Take(this);
        }
    }
}
