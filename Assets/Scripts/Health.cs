using System;
using UnityEngine;

public partial class Enemy
{
    public class Health
    {
        private float maxHealth;
        private float currHealth;

        public event Action OnDeath;
        public event Action<float> OnDamageTaken;

        public Health(float maxHealth)
        {
            this.maxHealth = maxHealth;
            currHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            currHealth = Mathf.Clamp(currHealth - damage, 0f, maxHealth);
            OnDamageTaken?.Invoke(currHealth);

            if (currHealth <= 0f)
            {
                OnDeath?.Invoke();
            }
        }

        public void Reset()
        {
            currHealth = maxHealth;
        }
    }
}
