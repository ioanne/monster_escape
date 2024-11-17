using UnityEngine;
using System;
using System.Collections;

public class HealthSystem
{
    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;

    private int maxHealth;
    private int currentHealth;

    private float regenerationInterval;
    private int regenerationAmount;
    private MonoBehaviour owner;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    public HealthSystem(int maxHealth, MonoBehaviour owner, float regenInterval = 5f, int regenAmount = 1)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
        this.owner = owner;
        regenerationInterval = regenInterval;
        regenerationAmount = regenAmount;

        owner.StartCoroutine(RegenerateHealth());
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private IEnumerator RegenerateHealth()
    {
        while (currentHealth > 0)
        {
            yield return new WaitForSeconds(regenerationInterval);
            Heal(regenerationAmount);
        }
    }
}
