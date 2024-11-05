using UnityEngine;

public class AudioController : MonoBehaviour, IVolumeControl, IMuteControl
{
    private static AudioController _instance;
    public static AudioController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioController>();
                if (_instance == null)
                {
                    GameObject audioControllerObject = new GameObject("AudioController");
                    _instance = audioControllerObject.AddComponent<AudioController>();
                    DontDestroyOnLoad(audioControllerObject);
                }
            }
            return _instance;
        }
    }

    private AudioListener _audioListener;
    private bool _isMuted = false;
    private float _currentVolume = 1.0f; // Almacena el volumen general (1.0f es el 100%)

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            // Busca el AudioListener en la escena
            _audioListener = FindObjectOfType<AudioListener>();
            if (_audioListener == null)
            {
                Debug.LogWarning("No se encontró un AudioListener en la escena. Asegúrate de que uno esté presente.");
            }
        }
    }

    // Método para ajustar el volumen en porcentaje (0-100)
    public void SetVolume(float volumePercentage)
    {
        _currentVolume = Mathf.Clamp(volumePercentage / 100f, 0f, 1f);
        ApplyVolume();
    }

    // Método para alternar entre silenciar y desilenciar
    public void ToggleMute()
    {
        _isMuted = !_isMuted;
        ApplyVolume();
    }

    // Método para obtener el volumen en porcentaje
    public float GetVolumePercentage()
    {
        return _currentVolume * 100f;
    }

    // Método para verificar si el audio está silenciado
    public bool IsMuted()
    {
        return _isMuted;
    }

    // Aplica el volumen en todos los AudioSources de la escena según el volumen actual y el estado de silencio
    private void ApplyVolume()
    {
        float volumeScale = _isMuted ? 0f : _currentVolume;

        // Ajusta todos los AudioSources en la escena
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource source in audioSources)
        {
            source.volume = volumeScale;
        }
    }
}
