using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    [Header("General Stats")]
    [SerializeField] private int maxHp = 100;
    [SerializeField] private int defense = 3;

    [Header("Attack Stats")]
    [SerializeField] private float attackPower = 10f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float attackSpeed = 2f;
    [SerializeField] private float regenerationInterval = 4f; // Intervalo de regeneración
    [SerializeField] private int regenerationAmount = 1; // Cantidad de vida que se regenera

    public int CurrentHealth => healthSystem.CurrentHealth;
    public int MaxHealth => healthSystem.MaxHealth;

    private HealthSystem healthSystem;

    public event Action OnEnemyDeath;

    private void Awake()
    {
        healthSystem = new HealthSystem(maxHp, this, regenerationInterval, regenerationAmount);

        healthSystem.OnHealthChanged += UpdateHealthUI;
        healthSystem.OnDeath += Die;
    }

    public void TakeDamage(int damage)
    {
        int finalDamage = Mathf.Max(damage - defense, 0);
        healthSystem.TakeDamage(finalDamage);
        Debug.Log("Enemy Takes Damage: " + finalDamage);
    }

    public bool IsDead()
    {
        return healthSystem.CurrentHealth <= 0;
    }

    private void Die()
    {
        Debug.Log("Enemy Die");
        OnEnemyDeath?.Invoke();
        GetComponent<Animator>().SetBool("IsDeath", true);
        Destroy(gameObject, 2f);
    }

    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        // Verificar si UIManager está disponible antes de actualizar la barra de vida
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateEnemyHealthBar(this, currentHealth, maxHealth);
        }
        else
        {
            Debug.LogWarning("UIManager.Instance is null. Enemy health UI could not be updated.");
        }

        Debug.Log($"Enemy Health updated: {currentHealth}/{maxHealth}");
    }

    public float GetAttackPower()
    {
        return attackPower;
    }

    public float GetAttackCooldown()
    {
        return attackCooldown;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }
}
