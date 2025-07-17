using UnityEngine;
using UnityEngine.UI;

public class InventoryToggle : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;

    [SerializeField] private Button closeButton;

    private bool isInventoryVisible = false;

    void Start()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseInventory);
            Debug.Log("Button listener added.");
        }
        else
        {
            Debug.LogError("Close button is not assigned.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isInventoryVisible = !isInventoryVisible;
            inventoryPanel.SetActive(isInventoryVisible);
        }
    }

    void CloseInventory()
    {
        Debug.Log("Toca boton");
        isInventoryVisible = false;
        inventoryPanel.SetActive(isInventoryVisible);
    }
}
