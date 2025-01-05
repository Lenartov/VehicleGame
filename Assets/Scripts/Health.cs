using System;
using UnityEngine;
using Microlight.MicroBar;


public class Health
{
    private MicroBar bar;
    private float maxHealth;
    private bool alwaysVisible;

    private float currHealth;

    public event Action OnDeath;
    public event Action<float> OnDamageTaken;

    public Health(float maxHealth, MicroBar bar, bool alwaysVisible = true)
    {
        this.bar = bar;
        this.maxHealth = maxHealth;
        currHealth = maxHealth;

        this.bar.Initialize(maxHealth);
        bar.OnMaxValueChange += OnReinit;
        this.alwaysVisible = alwaysVisible;
    }

    public void TakeDamage(float damage)
    {
        currHealth = Mathf.Clamp(currHealth - damage, 0f, maxHealth);
        OnDamageTaken?.Invoke(currHealth);
        bar.gameObject.SetActive(true);
        bar.UpdateBar(currHealth);

        if (currHealth <= 0f)
        {
            OnDeath?.Invoke();
        }
    }

    public void Reset()
    {
        currHealth = maxHealth;
        bar.Initialize(maxHealth);
    }

    private void OnReinit(MicroBar bar)
    {
        if (alwaysVisible)
            return;

        bar.gameObject.SetActive(false);
    }
}
