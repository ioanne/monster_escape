using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button menuButton;

    private void Start()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseInventory);
        }


        if (menuButton != null)
        {
            menuButton.onClick.AddListener(ToggleInventory);
        }
    }

    private void ToggleInventory()
    {
        UIManager.Instance.TogglePanel(Panel);
    }

    private void CloseInventory()
    {
        UIManager.Instance.HidePanel(Panel);
    }
}
