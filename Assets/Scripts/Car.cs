using DG.Tweening;
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

    private Rigidbody rb;
    private Coroutine movementCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    [ContextMenu("Start")]
    public void StartMovement()
    {
        if (movementCoroutine != null)
            return;

        movementCoroutine = StartCoroutine(Starting());
    }

    [ContextMenu("Stop")]
    public void StopMovement()
    {
        if (movementCoroutine != null)
            StopCoroutine(movementCoroutine);
        
        movementCoroutine = StartCoroutine(Stoping());
    }

    public Vector3 GetVelocity()
    {
        return rb.velocity;
    }

    private IEnumerator Starting()
    {
        for (float alpha = 0f; alpha < 1f; alpha += Time.fixedDeltaTime / accelerationDuration)
        {
            rb.velocity = transform.forward * (accelerationAnim.Evaluate(alpha) * speed);

            yield return null;
        }

        while (true)
        {
            rb.velocity = speed * transform.forward;
            yield return null;
        }
    }


    private IEnumerator Stoping()
    {
        float maxReachedSpeed = rb.velocity.z;
        for (float alpha = 0f; alpha < 1f; alpha += Time.fixedDeltaTime / stopDuration)
        {
            rb.velocity = transform.forward * (stopAnim.Evaluate(alpha) * maxReachedSpeed);

            yield return null;
        }

        rb.velocity = Vector3.zero;
        movementCoroutine = null;
    }
}
