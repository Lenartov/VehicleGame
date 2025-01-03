using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Redcode.Pools;
using System.Collections;
using UnityEngine;

public partial class Enemy : MonoBehaviour, IPoolObject
{
    [SerializeField] private float maxHealth;

    private Pool<Enemy> pool;
    private Coroutine delayedReturn;

    private EnenyAnimController animController;
    private Health health;

    private Car playerCar;
    private Coroutine playerDetection;

    private Rigidbody rb;

    private Coroutine idling;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Animator animator = GetComponent<Animator>();
        animController = new EnenyAnimController(animator);
        health = new Health(maxHealth);
        health.OnDeath += Die;
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
        CancelDelayedReturn();
        CancelPlayerDetection();
        CancelIdling();
        health.Reset();
        animController.Reset();
    }

    public void Init(Pool<Enemy> newPool, Car car)
    {
        pool = newPool;
        ReturnToPoolAfterDelay();

        playerCar = car;
        StartIdling();
        StartPlayerDetection();
    }

    private void ApplyDamage(float damage)
    {
        animController.OnGetHit();
        health.TakeDamage(damage);
    }

    private void Die()
    {
        Debug.Log("die");
        CancelDelayedReturn();
        CancelPlayerDetection();
        pool.Take(this);
    }

    private void CancelDelayedReturn()
    {
        if (delayedReturn != null)
            StopCoroutine(delayedReturn);
    }

    private void CancelPlayerDetection()
    {
        if (playerDetection != null)
            StopCoroutine(playerDetection);
    }

    private void CancelIdling()
    {
        if(idling != null)
            StopCoroutine(idling);

        rb.DOKill();
    }

    private void ReturnToPoolAfterDelay()
    {
        CancelDelayedReturn();
        delayedReturn = StartCoroutine(ReturningToPoolAfterDelay());
    }

    private void StartPlayerDetection()
    {
        CancelPlayerDetection();
        playerDetection = StartCoroutine(PlayerDetecting());
    }

    private void StartIdling()
    {
        CancelIdling();
        idling = StartCoroutine(Idling());
    }

    private IEnumerator ReturningToPoolAfterDelay()
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

    private IEnumerator Idling()
    {
        animController.SetRootMotion(true);
        bool isStanding = Random.Range(0f, 1f) >= 0.5f ? true : false;
        while (true)
        {
            animController.SetIdleStandingOrWalking(isStanding);

            if (isStanding)
            {
                yield return new WaitForSeconds(Random.Range(3f, 4f));
                isStanding = false;
            }
            else
            {
                Vector3 posToLook = transform.position + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
                transform.DORotate(Quaternion.LookRotation(posToLook - transform.position, Vector3.up).eulerAngles, 0.3f);

                yield return new WaitForSeconds(Random.Range(3f, 4f));
                isStanding = true;
            }
        }
    }

    private IEnumerator PlayerDetecting()
    {
        WaitForSeconds delay = new WaitForSeconds(0.1f);
        float requiredDistance = 35f + Random.Range(-7f, 7f);
        while (true)
        {
            bool isCloseEnought = Vector3.Distance(transform.position, playerCar.transform.position) < requiredDistance;

            if (isCloseEnought)
            {
                Vector3 posToLook = playerCar.transform.position - transform.position;
                transform.DORotate(Quaternion.LookRotation(posToLook - transform.position, Vector3.up).eulerAngles, 0.2f);

                StartCoroutine(Chasing());
            }
            yield return delay;
        }

    }

    private IEnumerator Chasing()
    {
        while (true)
        {
            //while (Vector3.Distance(rb.position, playerCar.transform.position) > 1f)
            {
                animController.SetRootMotion(false);
                animController.SetIsPlayerDetected(true);

                rb.velocity = (playerCar.transform.position - rb.position).normalized * 8f;
                yield return new WaitForSeconds(0.1f);
            }

           // rb.velocity = Vector3.zero;
           // animController.SetCloseToPlayer(true);

           // yield return new WaitForSeconds(0.1f);

        }
    }
}
