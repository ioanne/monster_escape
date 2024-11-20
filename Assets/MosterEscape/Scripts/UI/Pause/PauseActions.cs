using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseActions : MonoBehaviour
{
    [Header("Botones del Menú de Pausa")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button nextSceneButton;

    private List<string> levels = new List<string> { "Level1", "Level2", "Level3" };

    void Start()
    {
        AssignButtonActions();
    }

    private void AssignButtonActions()
    {
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(ResumeGame);
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }

        if (nextSceneButton != null)
        {
            nextSceneButton.onClick.AddListener(NextSceneGame);
        }
    }

    private void ResumeGame()
    {
        FindObjectOfType<PauseManager>()?.ResumeGame();
    }

    public void NextSceneGame()
    {
        Time.timeScale = 1f;

        string currentSceneName = GetCurrentLevelName();
        if (string.IsNullOrEmpty(currentSceneName))
        {
            Debug.LogWarning("El nivel actual no es parte de los niveles conocidos.");
            return;
        }

        string nextSceneName = GetNextSceneName(currentSceneName);

        LoadSceneManager.instance.LoadNextLevel(currentSceneName, nextSceneName);
        ResumeGame();
    }


    public void RestartGame()
    {
        Debug.Log("Reiniciar el juego.");
        Time.timeScale = 1f;

        string currentSceneName = GetCurrentLevelName();
        if (string.IsNullOrEmpty(currentSceneName))
        {
            Debug.LogWarning("El nivel actual no es parte de los niveles conocidos.");
            return;
        }

        LoadSceneManager.instance.LoadNextLevel(currentSceneName, currentSceneName);
        ResumeGame();
    }

    private string GetCurrentLevelName()
    {
        int sceneCount = SceneManager.sceneCount;

        for (int i = 0; i < sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            string activeSceneName = scene.name;

            foreach (string level in levels)
            {
                if (activeSceneName == level)
                {
                    return activeSceneName;
                }
            }
        }

        return null;
    }

    private string GetNextSceneName(string currentSceneName)
    {
        int currentIndex = levels.IndexOf(currentSceneName);

        if (currentIndex == -1)
        {
            Debug.LogWarning("No se encontró el nivel actual en la lista.");
            return levels[0];
        }

        int nextIndex = (currentIndex + 1) % levels.Count;

        return levels[nextIndex];
    }
}
