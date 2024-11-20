using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BootstrapLoader : MonoBehaviour
{
    [SerializeField] private Button playButton; // Botón para iniciar la carga de la escena
    [SerializeField] private GameObject menuToHide;
    void Start()
    {
        if (playButton != null)
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }
        else
        {
            Debug.LogError("Play button not assigned in the inspector.");
        }
    }

    // Método que se ejecuta cuando se hace clic en el botón "Jugar"
    void OnPlayButtonClicked()
    {
        // Iniciar la carga de la escena de carga
        StartCoroutine(LoadLoadingScene());
    }

    IEnumerator LoadLoadingScene()
    {
        // Cargar la escena de carga
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);

        // Esperar hasta que la escena esté completamente cargada
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        Task.Delay(100).Wait();
        Debug.Log("LoadingScene loaded.");

        // Desactivar el menú después de cargar la escena de carga
        if (menuToHide != null)
        {
            menuToHide.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Menu to hide is not assigned.");
        }

        // Ahora que la escena de carga está lista, comenzar a cargar las escenas principales
        if (LoadSceneManager.instance != null)
        {
            List<string> scenesToLoad = new List<string> { "PlayerUIScene", "Level1" };
            LoadSceneManager.instance.LoadScenes(scenesToLoad);
        }
        else
        {
            Debug.LogError("LoadSceneManager instance is null even after LoadingScene is loaded.");
        }
    }
}