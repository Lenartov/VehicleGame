using Redcode.Pools;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolObject
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;

    public float Damage => damage;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Shoot(Vector3 dir, Vector3 initVelocity)
    {
        rb.velocity += dir * speed + initVelocity;
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
