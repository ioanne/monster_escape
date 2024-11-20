using UnityEngine;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour
{
    [SerializeField] private Button exitButton; // Referencia al botón

    private void Start()
    {
        // Vincula la función QuitGame al botón si la referencia está asignada
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(QuitGame);
        }
        else
        {
            Debug.LogError("No se asignó el botón de salida en el inspector.");
        }
    }

    public void QuitGame()
    {
        // Si estamos ejecutando el juego desde el editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Si es una compilación, cerramos la aplicación
        Application.Quit();
#endif
    }
}