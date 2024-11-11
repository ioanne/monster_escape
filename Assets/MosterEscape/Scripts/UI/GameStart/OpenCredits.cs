using UnityEngine;
using UnityEngine.UI;

public class GoToMenu : MonoBehaviour
{
    [SerializeField] private Button creditsButton;
    [SerializeField] private GameObject creditsInfo;
    [SerializeField] private Button closeCreditsButton; // Botón para cerrar los créditos

    void Start()
    {
        if (creditsButton != null)
        {
            creditsButton.onClick.AddListener(ShowCredits);
        }

        if (closeCreditsButton != null)
        {
            closeCreditsButton.onClick.AddListener(HideCredits);
        }

        // Asegúrate de que los créditos estén ocultos al iniciar
        if (creditsInfo != null)
        {
            creditsInfo.SetActive(false);
        }
    }

    void ShowCredits()
    {
        if (creditsInfo != null)
        {
            creditsInfo.SetActive(true);
        }
    }

    void HideCredits()
    {
        if (creditsInfo != null)
        {
            creditsInfo.SetActive(false);
        }
    }
}
