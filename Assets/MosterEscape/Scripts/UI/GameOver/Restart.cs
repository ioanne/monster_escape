using UnityEngine;
using UnityEngine.UI;

public class Restart : MonoBehaviour
{
    [SerializeField] private Button restartButton;

    void Start()
    {
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartButtonClicked);

            // Asegurarse de que el cursor esté visible cuando aparece el botón
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Debug.LogWarning("RestartButton no está asignado en el inspector.");
        }
    }

    void RestartButtonClicked()
    {
        LoadSceneManager.instance.LoadSceneSynchronously("BootstrapScene");
    }
}
