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
                UIManager.Instance.ShowDoorInteractionMessage(); // Mostrar el mensaje si la puerta está dentro del rango

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Opening/Closing door: " + closestDoor.name);
                    closestDoor.Interact();
                }
            }
            else
            {
                UIManager.Instance.HideDoorInteractionMessage(); // Ocultar el mensaje si la puerta está fuera del rango
            }
        }
        else
        {
            UIManager.Instance.HideDoorInteractionMessage(); // Ocultar el mensaje si no hay puerta cerca
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
        UIManager.Instance.UpdateHealthBar(currentHealth, maxHealth);
        Debug.Log($"Health updated: {currentHealth}/{maxHealth}");
    }
}
