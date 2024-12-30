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

    private void Start()
    {
        int poolCapacity = 12;
        GameObject storage = new GameObject("Bullet Storage");
        bulletPool = Pool.Create(bulletPrefab, poolCapacity, storage.transform).NonLazy();
    }

    private void Update()
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
        pos.y = transform.position.y;
        transform.LookAt(pos);
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
            bullet.transform.position = muzzle.transform.position;

            if(car == null)
                bullet.Shoot(muzzle.transform.forward, Vector3.zero);
            else
                bullet.Shoot(muzzle.transform.forward, car.GetCurrVelocity());

            StartCoroutine(TakingBullets(bullet));
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
