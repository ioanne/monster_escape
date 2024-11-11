using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private GameObject enemyHealthBar;
    [SerializeField] private GameObject playerHealthBar;


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

    // Método para mostrar la barra de vida del enemigo
    public void ShowEnemyHealthBar()
    {
        if (enemyHealthBar != null)
        {
            enemyHealthBar.SetActive(true);
        }
    }

    // Método para ocultar la barra de vida del enemigo
    public void HideEnemyHealthBar()
    {
        if (enemyHealthBar != null)
        {
            enemyHealthBar.SetActive(false);
        }
    }

    // Método para actualizar la barra de salud del enemigo
    public void UpdateEnemyHealthBar(int currentHealth, int maxHealth)
    {
        if (enemyHealthBar != null)
        {
            Slider enemyHealthSlider = enemyHealthBar.GetComponent<Slider>();
            enemyHealthSlider.maxValue = maxHealth;
            enemyHealthSlider.value = currentHealth;
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
            // text mesh pro
            playerHealthText.text = $"{currentHealth}/{maxHealth}";
        }
    }
}
