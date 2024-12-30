using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private float speed;
    [Space]
    [SerializeField] private AnimationCurve accelerationAnim;
    [SerializeField] private float accelerationDuration;
    [Space]
    [SerializeField] private AnimationCurve stopAnim;
    [SerializeField] private float stopDuration;

    private Coroutine movementCoroutine;
    private float currentSpeed;

    [ContextMenu("Start")]
    public void StartMovement()
    {
        if (movementCoroutine != null)
            return;

        movementCoroutine = StartCoroutine(Accelerating());
    }

    [ContextMenu("Stop")]
    public void StopMovement()
    {
        if (movementCoroutine != null)
            StopCoroutine(movementCoroutine);
        
        movementCoroutine = StartCoroutine(Stoping());
    }

    public Vector3 GetCurrVelocity()
    {
        return currentSpeed * transform.forward;
    }

    private IEnumerator Accelerating()
    {
        currentSpeed = 0f;
        for (float alpha = 0f; alpha < 1f; alpha += Time.deltaTime/ accelerationDuration)
        {
            currentSpeed = accelerationAnim.Evaluate(alpha) * speed;
            transform.Translate(transform.forward * (Time.deltaTime * currentSpeed));

            yield return null;
        }
        currentSpeed = speed;

        yield return Moving();
    }


    private IEnumerator Moving()
    {
        currentSpeed = speed;
        while (true)
        {
            transform.Translate(transform.forward * (Time.deltaTime * currentSpeed));

            yield return null;
        }
    }

    private IEnumerator Stoping()
    {
        float maxReachedSpeed = currentSpeed;
        for (float alpha = 0f; alpha < 1f; alpha += Time.deltaTime / stopDuration)
        {
            currentSpeed = stopAnim.Evaluate(alpha) * maxReachedSpeed;
            transform.Translate(transform.forward * (Time.deltaTime * currentSpeed));

            yield return null;
        }

        currentSpeed = 0f;
        movementCoroutine = null;
    }
}
