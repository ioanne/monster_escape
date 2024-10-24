using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapLoader : MonoBehaviour
{
    void Start()
    {
        // Cargar primero la escena de loading
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
            List<string> scenesToLoad = new List<string> { "GameStartScene" };
            LoadSceneManager.instance.LoadScenes(scenesToLoad);
        }
        else
        {
            Debug.LogError("LoadSceneManager instance is null even after LoadingScene is loaded.");
        }
    }
}
