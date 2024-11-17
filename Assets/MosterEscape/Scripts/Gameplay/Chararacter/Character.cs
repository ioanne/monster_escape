using UnityEngine;

public class Character : MonoBehaviour
{
    private HealthSystem healthSystem;
    private CharacterStats characterStats;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int strength = 10;
    [SerializeField] private int defense = 5;
    [SerializeField] private float speed = 5f;
    [SerializeField] private int attackDamage = 20;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float regenerationInterval = 3f; // Intervalo de regeneración
    [SerializeField] private int regenerationAmount = 2; // Cantidad de vida que se regenera

    void Awake()
    {
        characterStats = new CharacterStats(strength, defense, speed, maxHealth);
        healthSystem = new HealthSystem(maxHealth, this, regenerationInterval, regenerationAmount);

        healthSystem.OnHealthChanged += UpdateHealthUI;
        healthSystem.OnDeath += HandleDeath;
    }

    public void TakeDamage(int damage)
    {
        healthSystem.TakeDamage(damage);
    }

    public void Heal(int healAmount)
    {
        healthSystem.Heal(healAmount);
    }

    private void HandleDeath()
    {
        Debug.Log("Character has died.");
        string sceneName = "GameOverScene";
        LoadSceneManager.instance.LoadSceneSynchronously(sceneName);
    }

    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        UIManager.Instance.UpdateHealthBar(currentHealth, maxHealth);
        Debug.Log($"Health updated: {currentHealth}/{maxHealth}");
    }
}
