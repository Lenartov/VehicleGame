using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] LayerMask aimMask;
    [Space]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate;

    private Coroutine shooting;
    private bool isActivated = true; //temp

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
            StopCoroutine(Shooting());
    }

    private IEnumerator Shooting()
    {
        while (true)
        {


            yield return null;
        }
    }
}
