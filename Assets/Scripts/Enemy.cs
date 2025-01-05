using DG.Tweening;
using Microlight.MicroBar;
using Redcode.Pools;
using System.Collections;
using UnityEngine;


public class Enemy : MonoBehaviour, IPoolObject, IDamagable
{
    [SerializeField] private MicroBar healthBar;
    [SerializeField] private float maxHealth;
    [Space]
    [SerializeField] private float damage;
    [Space]
    [SerializeField] private float desiredSpeed;
    [SerializeField] private AnimationCurve acceleration;

    private Rigidbody rb;
    private SkinnedMeshRenderer meshRenderer;
    private EnenyAnimController animController;
    private Health health;

    private Pool<Enemy> pool;
    private Car playerCar;

    private Coroutine playerDetection;
    private Coroutine delayedReturn;
    private Coroutine idling;

    private Vector2 minMaxPlayerTargetDistance;
    private float movementSpeedAlpha = 0f;
    private bool isTargetDetected;
    private bool isAttacking;
    private Color initColor;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        initColor = meshRenderer.material.color;

        Animator animator = GetComponent<Animator>();
        animController = new EnenyAnimController(animator);

        health = new Health(maxHealth, healthBar, false);
        health.OnDeath += Die;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Bullet bullet))
        {
            TakeDamage(bullet.Damage);
            bullet.Hit();

            if (isAttacking)
                return;

            GetHitImpact(bullet);
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
        isTargetDetected = false;
    }

    public void Init(Pool<Enemy> newPool, Car car, Vector2 minMaxTargetDistance)
    {
        playerCar = car;
        this.minMaxPlayerTargetDistance = minMaxTargetDistance;
        pool = newPool;
        ReturnToPoolAfterDelay(10f);

        StartIdling();
        StartPlayerDetection();
    }

    public void TakeDamage(float damage)
    {
        health.TakeDamage(damage);
        animController.OnGetHit();
        FlashMaterial();

        if (!isTargetDetected)
        {
            isTargetDetected = true;
            StartChase();
        }
    }

    public void Die()
    {
        CancelDelayedReturn();
        CancelPlayerDetection();
        pool.Take(this);
    }

    private void Attack()
    {
        isAttacking = true;
        animController.SetCloseToPlayer(true);

        StartCoroutine(ApplyDamageWithDelay(0.5f));
        ReturnToPoolAfterDelay(0.6f);
    }

    private void GetHitImpact(Bullet bullet)
    {
        float impactForce = 20f;
        rb.AddForce(bullet.ShootDiraction * impactForce, ForceMode.Impulse);
        movementSpeedAlpha = 0f;
    }

    private void FlashMaterial()
    {
        float duration = 0.15f;
        meshRenderer.material.DOColor(Color.white, duration).SetEase(Ease.OutFlash)
            .OnComplete(() => 
            {
                meshRenderer.material.DOColor(initColor, duration).SetEase(Ease.OutCubic);
            });
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

    private void StartChase()
    {
        isTargetDetected = true;
        CancelIdling();
        StartCoroutine(Chasing());
        StartCoroutine(Looking());
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
                //set a random direction with Z in the range { -1; -0.8} and { 0.8; 1}
                //to avoid units going along the X - axis too often, and going beyond the screen
                int sine = Random.Range(0f, 1f) >= 0.5f ? -1 : 1; 
                Vector3 posToLook = new Vector3(Random.Range(-0.2f, 0.2f), 0f, (0.8f + Random.Range(0f, 0.2f)) * sine);    
                transform.DORotate(Quaternion.LookRotation(posToLook, Vector3.up).eulerAngles, 0.2f);

                yield return new WaitForSeconds(Random.Range(3f, 4f));
                isStanding = true;
            }
        }
    }

    private IEnumerator PlayerDetecting()
    {
        WaitForSeconds delay = new WaitForSeconds(0.1f);
        float requiredDistance = Random.Range(minMaxPlayerTargetDistance.x, minMaxPlayerTargetDistance.y);
        while (true)
        {
            yield return delay;

            bool isCloseEnought = Vector3.Distance(transform.position, playerCar.transform.position) < requiredDistance;
            if (isCloseEnought)
            {
                StartChase();
                break;
            }
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

    private IEnumerator ApplyDamageWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerCar.TakeDamage(damage);
    }
}
