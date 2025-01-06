using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public partial class Ground : MonoBehaviour
{
    [SerializeField] private GroundPart groundPartPrefab;
    [SerializeField] private int partsCount;
    [Space]
    [SerializeField] private GameObject finishLinePrefab;
    [SerializeField] private float levelLength = 1f;
    [Space]
    [SerializeField] private float desiredSpeed;
    [SerializeField] private float accelerationDuration;
    [SerializeField] private float stopDuration;

    public event Action OnWin;
    public event Action<float> OnDistanceChanged;

    private GroundSpawner groundSpawner;

    private Vector3 initPos;
    private float currentSpeed = 0f;
    private float distanceCovered = 0f;
    private bool isFinished;

    private void Awake()
    {
        groundSpawner = new GroundSpawner(groundPartPrefab, finishLinePrefab);
        groundSpawner.SpawnGround(transform, partsCount, levelLength);
        initPos = transform.position;
    }

    private void Update()
    {
        if (currentSpeed <= 0.05f)
            return;

        transform.Translate(Vector3.forward * (-currentSpeed * Time.deltaTime));

        if (isFinished)
            return;

        distanceCovered = initPos.z - transform.position.z;
        OnDistanceChanged?.Invoke(distanceCovered / levelLength);
        if (distanceCovered >= levelLength)
        {
            isFinished = true;
            OnWin?.Invoke();
        }
    }

    public void Restart()
    {
        groundSpawner.Clear();
        groundSpawner.SpawnGround(transform, partsCount, levelLength);
        transform.position = initPos;
        distanceCovered = 0f;
        OnDistanceChanged?.Invoke(distanceCovered / levelLength);
    }

    public void Acceleration()
    {
        DOTween.To(() => currentSpeed, x => currentSpeed = x, desiredSpeed, accelerationDuration).SetEase(Ease.InCubic);
    }

    public void Stop()
    {
        DOTween.To(() => currentSpeed, x => currentSpeed = x, 0f, stopDuration).SetEase(Ease.OutSine);
    }


}
