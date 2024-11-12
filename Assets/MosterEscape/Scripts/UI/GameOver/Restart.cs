using UnityEngine;
using UnityEngine.UI;

public class Restart : MonoBehaviour
{
    [SerializeField] private Button RestartButton;
    void Start()
    {
        if (RestartButton != null)
        {
            RestartButton.onClick.AddListener(RestartButtonClicked);
        }
    }

    void RestartButtonClicked()
    {
        LoadSceneManager.instance.LoadSceneSynchronously("BootstrapScene");
    }

}
