using UnityEngine;

public class Character : MonoBehaviour
{
    private HealthSystem healthSystem;
    private CharacterStats characterStats;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int strength = 10;
    [SerializeField] private int defense = 5;
    [SerializeField] private float speed = 5f;

    void Awake()
    {
        characterStats = new CharacterStats(strength, defense, speed, maxHealth);
        healthSystem = new HealthSystem(maxHealth);

        healthSystem.OnHealthChanged += UpdateHealthUI;
        healthSystem.OnDeath += HandleDeath;
    }

    // Método para recibir daño
    public void TakeDamage(int damage)
    {
        healthSystem.TakeDamage(damage);
    }

    // Método para curarse
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

    // Método para actualizar la UI
    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        UIManager.Instance.UpdateHealthBar(currentHealth, maxHealth);
        Debug.Log($"Health updated: {currentHealth}/{maxHealth}");
    }
}
