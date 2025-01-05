using DG.Tweening;
using System.Collections;
using UnityEngine;

public partial class Ground : MonoBehaviour
{
    [SerializeField] private GroundPart groundPartPrefab;
    [SerializeField] private int partsCount;
    [Space]
    [SerializeField] private float desiredSpeed;
    [SerializeField] private float accelerationDuration;
    [SerializeField] private float stopDuration;


    private GroundSpawner groundSpawner;

    private float currentSpeed = 0f;

    private void Awake()
    {
        groundSpawner = new GroundSpawner(groundPartPrefab);
        groundSpawner.SpawnGround(transform, partsCount);
    }

    private void Update()
    {
        if (currentSpeed <= 0.1f)
            return;

        transform.Translate(Vector3.forward * (-currentSpeed * Time.deltaTime));
    }

    public void Acceleration()
    {
        DOTween.To(() => currentSpeed, x => currentSpeed = x, desiredSpeed, accelerationDuration).SetEase(Ease.InCubic);
    }

    public void Stop()
    {
        DOTween.To(() => currentSpeed, x => currentSpeed = x, 0f, stopDuration).SetEase(Ease.OutCubic);
    }


}
