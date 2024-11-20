using UnityEngine;

public class ConditionalDoor : Door
{
    [SerializeField] private int requiredKeys = 1;
    [SerializeField] private bool requireAllEnemiesDead = true; 

    // Este método será llamado desde el Character para verificar las condiciones de la puerta
    public void CheckConditions(int playerKeys, bool enemiesDead)
    {
        if (playerKeys >= requiredKeys && (requireAllEnemiesDead ? enemiesDead : true))
        {
            Debug.Log("Conditions met, opening the door!");
            InventoryManager.Instance.UseItem("Key", requiredKeys);
          
            Interact();
        }
        else
        {
            Debug.Log("Conditions not met. Cannot open the door.");
        }
    }
}

