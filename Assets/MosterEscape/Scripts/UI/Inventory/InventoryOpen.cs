using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryOpen : MonoBehaviour
{
    [SerializeField] private Button openButton;

    [SerializeField] private GameObject inventoryPanel;

    private bool isInventoryVisible = false;


    void Start()
    {
        if (openButton != null)
        {
            openButton.onClick.AddListener(OpenInventory);
        }

    }

    void Update()
    {
        
    }

    void OpenInventory()
    {
        if (!isInventoryVisible)
        {
            isInventoryVisible = true;
            inventoryPanel.SetActive(isInventoryVisible);
        } else
        {
            isInventoryVisible = false;
            inventoryPanel.SetActive(isInventoryVisible);
        }
        
    }
}
