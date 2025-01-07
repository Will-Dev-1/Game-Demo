using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string itemName; 
    public int quantity = 1; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            InventorySystem inventory = other.GetComponent<InventorySystem>();
            if (inventory != null)
            {
                // Add item to the inventory system
                inventory.AddItem(itemName, quantity);
                Destroy(gameObject); // Remove the item from the scene after pickup
            }
        }
    }
}
