using DG.Tweening;
using Redcode.Pools;
using System.Collections;
using UnityEngine;

public partial class Enemy : MonoBehaviour, IPoolObject
{
    [SerializeField] private float maxHealth;
    [Space]
    [SerializeField] private float desiredSpeed;
    [SerializeField] private AnimationCurve acceleration;
    [Space]
    [SerializeField] private Vector2 minMaxPlayerDetectionDistance;

    private Rigidbody rb;
    private EnenyAnimController animController;
    private Health health;

    private Pool<Enemy> pool;
    private Car playerCar;

    private Coroutine playerDetection;
    private Coroutine delayedReturn;
    private Coroutine idling;

    private float movementSpeedAlpha = 0f;
    private bool isAttacking;

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
            TakeDamage(bullet);
            bullet.Hit();
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
        isAttacking = false;
    }

    public void Init(Pool<Enemy> newPool, Car car)
    {
        playerCar = car;

        pool = newPool;
        ReturnToPoolAfterDelay(10f);

        StartIdling();
        StartPlayerDetection();
    }

    private void TakeDamage(Bullet bullet)
    {
        health.TakeDamage(bullet.Damage);
        animController.OnGetHit();

        if (isAttacking)
            return;

        rb.AddForce(bullet.ShootDiraction * 20f, ForceMode.Impulse);
        movementSpeedAlpha = Mathf.Clamp01(movementSpeedAlpha - 1f);
    }

    private void Die()
    {
        //CancelDelayedReturn();
        //CancelPlayerDetection();
        pool.Take(this);
    }

    private void Attack()
    {
        isAttacking = true;
        animController.SetCloseToPlayer(true);
        ReturnToPoolAfterDelay(0.6f);
    }

    private void ReturnToPoolAfterDelay(float delay)
    {
        CancelDelayedReturn();
        delayedReturn = StartCoroutine(ReturningToPoolAfterDelay(delay));
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
        if (idling != null)
            StopCoroutine(idling);

        transform.DOKill();
    }

    private IEnumerator ReturningToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

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
                transform.DORotate(Quaternion.LookRotation(posToLook - transform.position, Vector3.up).eulerAngles, 0.2f);

                yield return new WaitForSeconds(Random.Range(3f, 4f));
                isStanding = true;
            }
        }
    }

    private IEnumerator PlayerDetecting()
    {
        WaitForSeconds delay = new WaitForSeconds(0.1f);
        float requiredDistance = Random.Range(minMaxPlayerDetectionDistance.x, minMaxPlayerDetectionDistance.y);
        while (true)
        {
            bool isCloseEnought = Vector3.Distance(transform.position, playerCar.transform.position) < requiredDistance;
            if (isCloseEnought)
            {
                CancelIdling();
                StartCoroutine(Chasing());
                StartCoroutine(Looking());
                break;
            }
            yield return delay;
        }
    }

    private IEnumerator Looking()
    {
        float rotateSpeed = 8f;
        while (true)
        {
            Vector3 posToLook = (playerCar.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(posToLook, Vector3.up), Time.deltaTime * rotateSpeed);
            yield return null;
        }
    }

    private IEnumerator Chasing()
    {
        WaitForSeconds delay = new WaitForSeconds(0.1f);
        float accelerationTime = 0.5f;
        float closeEnoughtDistance = 4f;
        float speedMultiplier;

        animController.SetRootMotion(false);
        animController.SetIsPlayerDetected(true);

        while (Vector3.Distance(rb.position, playerCar.transform.position) > closeEnoughtDistance)
        {
            speedMultiplier = acceleration.Evaluate(movementSpeedAlpha / accelerationTime);
            animController.SetSpeedMultiplier(speedMultiplier);
            rb.velocity = (playerCar.transform.position - rb.position).normalized * (speedMultiplier * desiredSpeed);

            yield return delay;
            movementSpeedAlpha = Mathf.Clamp01(movementSpeedAlpha + 0.1f);
        }
        Attack();
    }
}
