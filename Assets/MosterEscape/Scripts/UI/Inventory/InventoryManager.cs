using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private Dictionary<string, int> inventory = new Dictionary<string, int>();

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
        }
    }

    public bool UseItem(string itemName, int quantity)
    {
        if (inventory.ContainsKey(itemName) && inventory[itemName] >= quantity)
        {
            inventory[itemName] -= quantity;
            if (inventory[itemName] <= 0)
            {
                inventory.Remove(itemName);
            }
            UIInventoryManager.Instance.UpdateInventoryUI(itemName, inventory.ContainsKey(itemName) ? inventory[itemName] : 0);
            return true;
        }
        return false; // No hay suficiente del ítem
    }

    public int GetItemQuantity(string itemName)
    {
        return inventory.ContainsKey(itemName) ? inventory[itemName] : 0;
    }

    public void AddItem(string itemName, int quantity)
    {
        if (!inventory.ContainsKey(itemName))
        {
            inventory[itemName] = 0;
        }
        inventory[itemName] += quantity;
        UIInventoryManager.Instance.UpdateInventoryUI(itemName, inventory[itemName]);
    }
}

