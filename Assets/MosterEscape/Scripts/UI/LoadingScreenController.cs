using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour
{
    public static LoadingScreenController Instance;

    public Slider progressBar;
    public TMP_Text progressText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (transform.parent != null)
            {
                transform.parent = null;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateProgress(float progress)
    {
        // Actualiza la barra de progreso y el texto
        progressBar.value = progress * 100f;
        progressText.text = $"Loading... {progress * 100f:0}%";
    }
}
