using DG.Tweening;
using Microlight.MicroBar;
using System;
using UnityEngine;

public class Car : MonoBehaviour, IDamagable
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Color damageFlashColor;
    [Space]
    [SerializeField] private MicroBar healthBar;
    [SerializeField] private float maxHealth;
    [Space]
    [SerializeField] private Turret turret;
    [Space]
    [SerializeField] private ParticleSystem dieExplosionEffect;
    [SerializeField] private ParticleSystem cheerEffect;

    public event Action OnDie;

    private Health health;
    private bool isDead;
    private Color initColor;

    private void Awake()
    {
        initColor = meshRenderer.material.color;

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

    public void Cheer()
    {
        cheerEffect.Play(true);
    }

    public void TakeDamage(float damage)
    {
        health.TakeDamage(damage);
        FlashMaterial();
    }

    public void Restart()
    {
        isDead = false;
        health.Reset();
        cheerEffect.Clear();
        dieExplosionEffect.Clear();
    }

    private void FlashMaterial()
    {
        float duration = 0.1f;
        meshRenderer.material.DOColor(damageFlashColor, duration).SetEase(Ease.OutFlash)
            .OnComplete(() =>
            {
                meshRenderer.material.DOColor(initColor, duration).SetEase(Ease.OutCubic);
            });
    }
}
