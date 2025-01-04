using Redcode.Pools;
using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolObject
{
    [SerializeField] private TrailRenderer trail;
    [Space]
    [SerializeField] private float speed;
    [SerializeField] private float damage;

    public float Damage => damage;
    public Vector3 ShootDiraction { get; private set; }

    private Rigidbody rb;
    private Pool<Bullet> pool;
    private Coroutine delayedReturn;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Init(Pool<Bullet> pool)
    {
        this.pool = pool;
        TakeAfterDelay(3f);
    }

    public void Shoot(Vector3 dir)
    {
        trail.Clear();

        ShootDiraction = dir;
        rb.AddForce(dir * speed, ForceMode.Impulse);
    }

    public void Hit()
    {
        pool.Take(this);
    }

    public void OnCreatedInPool() {}

    public void OnGettingFromPool()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        rb.velocity = Vector3.zero;
    }

    public void TakeAfterDelay(float delay)
    {
        if (delayedReturn != null)
            StopCoroutine(delayedReturn);

        delayedReturn = StartCoroutine(TakingBulletAfterDelay(delay));
    }

    private IEnumerator TakingBulletAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        pool.Take(this);
        delayedReturn = null;
    }
}
