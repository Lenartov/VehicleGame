using Redcode.Pools;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IPoolObject
{
    [Space]
    [SerializeField] private float maxHealth;

    private Pool<Enemy> pool;
    private Coroutine delayedReturn;
    private EnenyAnimController animController;

    private float health;

    private void Awake()
    {
        Animator animator = GetComponent<Animator>();
        animController = new EnenyAnimController(animator);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Bullet bullet))
        {
            ApplyDamage(bullet.Damage);
        }
    }

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

    private void ReturningToPoolAfterDelay()
    {
        CancelDelayedReturn();
        delayedReturn = StartCoroutine(ReturningToPool());
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
