using System.Collections;
using System.Collections.Generic;
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
        if (menuToHide != null)
        {
            menuToHide.SetActive(false); // Ocultar el menú
        }
        else
        {
            Debug.LogWarning("Menu to hide is not assigned.");
        }

        StartCoroutine(LoadLoadingScene());
    }

    IEnumerator LoadLoadingScene()
    {
        // Cargar la escena de loading
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);

        // Esperar hasta que la escena esté cargada
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Ahora que la escena de loading está cargada, podemos acceder al LoadSceneManager
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