using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BootstrapLoader : MonoBehaviour
{
    [SerializeField] private Button playButton; // Botón para iniciar la carga de la escena

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
