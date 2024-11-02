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

public async void LoadScenes(List<string> sceneNames)
{
    Debug.Log("Starting to load scenes.");
    _loader.SetActive(true);

    float totalProgress = 0f;
    float progressPerScene = 1f / sceneNames.Count;
    List<AsyncOperation> scenesToActivate = new List<AsyncOperation>();

    // Cargar cada escena sin activarlas
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

    // Activar todas las escenas al mismo tiempo
    foreach (var scene in scenesToActivate)
    {
        scene.allowSceneActivation = true;
    }

    Debug.Log("All scenes loaded and activated.");
    _loader.SetActive(false);
}
}
