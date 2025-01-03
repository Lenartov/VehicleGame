using Redcode.Pools;
using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private Car car;
    [Space]
    [SerializeField] private LayerMask aimMask;
    [SerializeField] private GameObject muzzle;
    [Space]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float fireRate;


    private Pool<Bullet> bulletPool;
    private Coroutine shooting;
    private bool isActivated = true; //temp
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        InitPool();
    }

    private void InitPool()
    {
        int poolCapacity = 12;
        GameObject storage = new GameObject("Bullet Storage");
        bulletPool = Pool.Create(bulletPrefab, 0, storage.transform);

        for (int i = 0; i < poolCapacity; i++)
        {
            bulletPool.Take(Instantiate(bulletPrefab));
        }
    }

    private void FixedUpdate()
    {
        if(isActivated)
            LookAtPosition(PlayerInput.GetCursorPos(aimMask));
    }

    public void Activate()
    {
        isActivated = true;
    }

    public void Deactivate()
    {
        isActivated = false;
    }

    public void LookAtPosition(Vector3 pos)
    {
        Quaternion targetRotation = Quaternion.LookRotation((pos - transform.position).normalized);
        Quaternion smoothRotation = Quaternion.Lerp(rb.rotation, targetRotation, 20f * Time.fixedDeltaTime);

        rb.MoveRotation(smoothRotation);
    }

    public void StartShooting()
    {
        if (shooting != null)
            return;

        shooting = StartCoroutine(Shooting());
    }

    public void StopShooting()
    {
        if (shooting != null)
        {
            StopCoroutine(shooting);
            shooting = null;
        }
    }

    private IEnumerator Shooting()
    {
        float delayBetweenShoots = 60f / fireRate;
        WaitForSeconds delayWait = new WaitForSeconds(delayBetweenShoots);
        while (true)
        {
            Bullet bullet = bulletPool.Get();
            bullet.Init(bulletPool);
            bullet.transform.position = muzzle.transform.position;

            if (car == null)
                bullet.Shoot(muzzle.transform.forward, Vector3.zero);
            else
                bullet.Shoot(muzzle.transform.forward, car.GetVelocity());

            //StartCoroutine(TakingBullets(bullet));
            yield return delayWait;
        }
    }

    private IEnumerator TakingBullets(Bullet bullet)
    {
        float bulletLifeTime = 3f;
        yield return new WaitForSeconds(bulletLifeTime);

        bulletPool.Take(bullet);
    }
}
