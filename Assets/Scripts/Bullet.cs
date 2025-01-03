using Redcode.Pools;
using System;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolObject
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;

    public float Damage => damage;
    public Vector3 ShootDiraction { get; private set; }

    private Rigidbody rb;
    private Pool<Bullet> pool;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Init(Pool<Bullet> pool)
    {
        this.pool = pool;
    }

    public void Shoot(Vector3 dir, Vector3 initVelocity)
    {
        ShootDiraction = dir;
        rb.velocity += dir * speed + initVelocity;
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
}
