using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [Header("Menu de Pausa")]
    [SerializeField] private GameObject pauseMenu; // Panel del menú de pausa

    private bool isPaused = false;

    void Update()
    {
        // Detectar la tecla P para pausar o reanudar
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Pausar el tiempo
        pauseMenu.SetActive(true); // Mostrar el menú
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Reanudar el tiempo
        pauseMenu.SetActive(false); // Ocultar el menú
    }
}
