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
    private bool isActivated = true;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        InitPool();
    }

    private void FixedUpdate()
    {
        if(isActivated)
            LookAtPosition(PlayerInput.GetCursorPos(aimMask));
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

    public void Activate()
    {
        isActivated = true;
        muzzle.SetActive(true);
    }

    public void Deactivate()
    {
        isActivated = false;
        muzzle.SetActive(false);

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward);
        rb.MoveRotation(targetRotation);
        StopShooting();
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
            Bullet bullet = SpawnBullet();
            bullet.Shoot(muzzle.transform.forward);

            //triple bullets for fun

            /* Bullet bullet1 = SpawnBullet();
            bullet1.Shoot(Quaternion.AngleAxis(5f, Vector3.up) * muzzle.transform.forward);
            bullet1.transform.rotation = Quaternion.LookRotation(Quaternion.AngleAxis(5, Vector3.up) * muzzle.transform.forward, Vector3.up);

            Bullet bullet2 = SpawnBullet();
            bullet2.Shoot(Quaternion.AngleAxis(5f, Vector3.down) * muzzle.transform.forward);
            bullet2.transform.rotation = Quaternion.LookRotation(Quaternion.AngleAxis(5, Vector3.down) * muzzle.transform.forward, Vector3.up);
           */
            yield return delayWait;
        }
    }

    private Bullet SpawnBullet()
    {
        Bullet bullet = bulletPool.Get();
        bullet.Init(bulletPool);

        bullet.transform.position = muzzle.transform.position;
        bullet.transform.rotation = Quaternion.LookRotation(muzzle.transform.forward, Vector3.up);
        return bullet;
    }
}
