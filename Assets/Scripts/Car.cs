using Microlight.MicroBar;
using System;
using UnityEngine;

public class Car : MonoBehaviour, IDamagable
{
    [SerializeField] private MicroBar healthBar;
    [SerializeField] private float maxHealth;
    [Space]
    [SerializeField] private Turret turret;
    [Space]
    [SerializeField] private ParticleSystem dieExplosionEffect;

    public event Action OnDie;
    private Health health;
    private bool isDead;

    private void Awake()
    {
        health = new Health(maxHealth, healthBar);
        health.OnDeath += Die;
    }

    public void Activate()
    {
        healthBar.gameObject.SetActive(true);
        turret.Activate();
    }

    public void Deactivate()
    {
        healthBar.gameObject.SetActive(false);
        turret.Deactivate();
    }

    public void Die()
    {
        if (isDead)
            return;

        dieExplosionEffect.Play();
        isDead = true;
        OnDie?.Invoke();
    }

    public void TakeDamage(float damage)
    {
        health.TakeDamage(damage);
        //FlashMaterial();
    }

    public void Reset()
    {
        isDead = false;
        health.Reset();
    }
}
