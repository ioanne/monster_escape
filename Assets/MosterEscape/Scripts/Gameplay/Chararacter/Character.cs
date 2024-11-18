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
    [SerializeField] private float interactionRange = 4f; // Rango de interacción con la puerta
    [SerializeField] private float regenerationInterval = 3f; // Intervalo de regeneración
    [SerializeField] private int regenerationAmount = 2; // Cantidad de vida que se regenera

    void Awake()
    {
        characterStats = new CharacterStats(strength, defense, speed, maxHealth);
        healthSystem = new HealthSystem(maxHealth, this, regenerationInterval, regenerationAmount);

        healthSystem.OnHealthChanged += UpdateHealthUI;
        healthSystem.OnDeath += HandleDeath;
    }

    void Update()
    {
        HandleDoorInteraction();
    }

    public void TakeDamage(int damage)
    {
        healthSystem.TakeDamage(damage);
    }

    private void HandleDoorInteraction()
    {
        Door closestDoor = FindClosestDoor();
        if (closestDoor != null)
        {
            float distanceToDoor = Vector3.Distance(transform.position, closestDoor.transform.position);

            if (distanceToDoor <= interactionRange)
            {
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.ShowDoorInteractionMessage();
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Opening/Closing door: " + closestDoor.name);
                    closestDoor.Interact();
                }
            }
            else
            {
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.HideDoorInteractionMessage();
                }
            }
        }
        else
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.HideDoorInteractionMessage();
            }
        }
    }

    private Door FindClosestDoor()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRange);
        Door closestDoor = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("InteractableDoor"))
            {
                Door door = collider.GetComponent<Door>();
                if (door != null)
                {
                    float distance = Vector3.Distance(transform.position, door.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestDoor = door;
                    }
                }
            }
        }

        return closestDoor;
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
        // Verificar si el UIManager está disponible antes de actualizar la barra de vida
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealthBar(currentHealth, maxHealth);
        }
        else
        {
            Debug.LogWarning("UIManager.Instance is null. Health UI could not be updated.");
        }

        Debug.Log($"Health updated: {currentHealth}/{maxHealth}");
    }
}
