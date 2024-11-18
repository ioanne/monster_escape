using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource audioSource;

    [SerializeField] private AudioClip menuMusic;   // Música para el menú principal
    [SerializeField] private AudioClip gameplayMusic; // Música para el gameplay

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return; // Asegúrate de salir si no es la instancia principal
        }

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // Reproduce música inicial según la escena cargada al inicio
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);

        // Suscríbete al evento de cambio de escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Limpia el evento si se destruye el objeto
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Selecciona la música según el índice de la escena
        switch (scene.buildIndex)
        {
            case 1: // Índice de la escena MainMenu
                PlayMusic(menuMusic);
                break;
            case 6: // Índice de la escena Gameplay
                PlayMusic(gameplayMusic);
                break;
        }
    }


    public void PlayMusic(AudioClip music)
    {
        if (audioSource.clip == music) return; // Evita reiniciar la música si ya está sonando
        audioSource.clip = music;
        audioSource.loop = true; // Asegúrate de que la música se reproduzca en bucle
        audioSource.Play();
    }

    public void Playsound(AudioClip sonido)
    {
        audioSource.PlayOneShot(sonido);
    }

}
