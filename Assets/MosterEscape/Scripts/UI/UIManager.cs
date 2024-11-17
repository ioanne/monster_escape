using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private GameObject enemyHealthBar; // Prefab de la barra de salud del enemigo
    [SerializeField] private GameObject playerHealthBar;
    [SerializeField] private GameObject doorInteractionMessage;

    private Enemy targetEnemy; // El enemigo objetivo actualmente seleccionado

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TogglePanel(GameObject panel)
    {
        if (panel != null)
        {
            bool isActive = panel.activeSelf;
            panel.SetActive(!isActive);
        }
    }

    public void ShowPanel(GameObject panel)
    {
        if (panel != null)
        {
            panel.SetActive(true);
        }
    }

    public void HidePanel(GameObject panel)
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    public void ShowEnemyHealthBar(Enemy enemy)
    {
        targetEnemy = enemy; // Asigna el enemigo objetivo
        enemyHealthBar.SetActive(true); // Muestra la barra de salud
        UpdateEnemyHealthBar(targetEnemy, targetEnemy.CurrentHealth, targetEnemy.MaxHealth); // Actualiza la barra de salud
    }

    public void UpdateEnemyHealthBar(Enemy enemy, int currentHealth, int maxHealth)
    {
        if (targetEnemy == enemy && enemyHealthBar != null) // Asegúrate de que se actualice solo para el enemigo objetivo
        {
            Slider enemyHealthSlider = enemyHealthBar.GetComponentInChildren<Slider>();
            TextMeshProUGUI enemyHealthText = enemyHealthBar.GetComponentInChildren<TextMeshProUGUI>();

            enemyHealthSlider.maxValue = maxHealth;
            enemyHealthSlider.value = currentHealth;
            enemyHealthText.text = $"{currentHealth}/{maxHealth}";
        }
    }

    public void HideEnemyHealthBar(Enemy enemy)
    {
        Debug.Log("Call to hide enemy health bar");
        if (targetEnemy == enemy && enemyHealthBar != null) // Solo oculta si el enemigo coincide
        {
            Debug.Log("Hiding enemy health bar");
            enemyHealthBar.SetActive(false); // Oculta la barra de salud
            targetEnemy = null; // Reinicia el enemigo objetivo
        }
    }

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (playerHealthBar != null)
        {
            Slider playerHealthSlider = playerHealthBar.GetComponent<Slider>();
            TextMeshProUGUI playerHealthText = playerHealthBar.GetComponentInChildren<TextMeshProUGUI>();
            playerHealthSlider.maxValue = maxHealth;
            playerHealthSlider.value = currentHealth;
            playerHealthText.text = $"{currentHealth}/{maxHealth}";
        }
    }


    public void ShowDoorInteractionMessage()
    {
        if (doorInteractionMessage != null)
        {
            doorInteractionMessage.SetActive(true); // Muestra el mensaje
        }
    }

    public void HideDoorInteractionMessage()
    {
        if (doorInteractionMessage != null)
        {
            doorInteractionMessage.SetActive(false); // Oculta el mensaje
        }
    }
}
