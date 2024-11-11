using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;

public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager instance;
    [SerializeField] private GameObject _loader;
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI progressText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Método para descargar (unload) todas las escenas excepto la escena principal
    private async Task UnloadAllScenesAsync()
    {
        List<Scene> scenesToUnload = new List<Scene>();

        // Recopilar todas las escenas cargadas excepto la escena principal (índice 0)
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.buildIndex != 0)
            {
                scenesToUnload.Add(scene);
            }
        }

        // Descargar cada escena de la lista
        foreach (var scene in scenesToUnload)
        {
            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(scene);
            while (!unloadOperation.isDone)
            {
                await Task.Yield();
            }
        }

        Debug.Log("All scenes have been unloaded.");
    }

    // Método para cargar escenas de manera asincrónica
    public async void LoadScenes(List<string> sceneNames)
    {
        Debug.Log("Starting to load scenes.");
        _loader.SetActive(true);

        // Descargar todas las escenas antes de cargar las nuevas
        await UnloadAllScenesAsync();

        float totalProgress = 0f;
        float progressPerScene = 1f / sceneNames.Count;
        List<AsyncOperation> scenesToActivate = new List<AsyncOperation>();

        foreach (var sceneName in sceneNames)
        {
            Debug.Log($"Loading scene: {sceneName}");
            var scene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            scene.allowSceneActivation = false;
            scenesToActivate.Add(scene);

            while (!scene.isDone)
            {
                float sceneProgress = Mathf.Clamp01(scene.progress / 0.9f);
                progressBar.value = (totalProgress + sceneProgress * progressPerScene) * 100;
                progressText.text = $"Loading... {(progressBar.value):F0}%";

                Debug.Log($"Scene progress: {sceneProgress * 100:F0}%, Total progress: {progressBar.value:F0}%");

                if (scene.progress >= 0.9f)
                {
                    await Task.Delay(10);
                    break;
                }

                await Task.Yield();
            }

            Debug.Log($"Scene {sceneName} loaded to 90%.");
            totalProgress += progressPerScene;
        }

        await Task.Delay(200);

        // Activar todas las escenas cargadas
        foreach (var scene in scenesToActivate)
        {
            scene.allowSceneActivation = true;
        }
        await Task.Delay(300);

        Debug.Log("All scenes loaded and activated.");
        _loader.SetActive(false);
    }

    // Método para cargar una escena de manera sincrónica
    public void LoadSceneSynchronously(string sceneName)
    {
        Debug.Log($"Loading scene {sceneName} synchronously.");

        // // Descargar todas las escenas antes de cargar la nueva escena
        // UnloadAllScenesAsync().Wait();

        // // Mostrar la pantalla de carga
        // _loader.SetActive(true);

        // Cargar la escena sincrónicamente
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);

        // Ocultar la pantalla de carga
        _loader.SetActive(false);

        Debug.Log($"Scene {sceneName} loaded synchronously.");
    }
}
