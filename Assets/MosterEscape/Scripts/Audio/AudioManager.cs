using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource audioSource;

    [SerializeField] private AudioClip menuMusic;   // Musica para el menu principal
    [SerializeField] private AudioClip gameplayMusic; // Musica para el gameplay

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
            return; // Asegurate de salir si no es la instancia principal
        }

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // Reproduce musica inicial segun la escena cargada al inicio
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);

        // Suscribete al evento de cambio de escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Limpia el evento si se destruye el objeto
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Selecciona la musica segun el indice de la escena
        switch (scene.buildIndex)
        {
            case 1: // indice de la escena MainMenu
                PlayMusic(menuMusic);
                break;
            case 3: // indice de la escena Gameplay
                PlayMusic(gameplayMusic);
                break;
        }
    }


    public void PlayMusic(AudioClip music)
    {
        if (audioSource.clip == music) return; // Evita reiniciar la musica si ya esta sonando
        audioSource.clip = music;
        audioSource.loop = true; // Asegurate de que la musica se reproduzca en bucle
        audioSource.Play();
    }

    public void Playsound(AudioClip sonido)
    {
        audioSource.PlayOneShot(sonido);
    }

}
