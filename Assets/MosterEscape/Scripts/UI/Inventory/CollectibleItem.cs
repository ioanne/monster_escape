using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private int quantity = 1;
    [SerializeField] private AudioClip collect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.Playsound(collect);
            InventoryManager.Instance.AddItem(itemName, quantity);
            Destroy(gameObject);
        }
    }
}

