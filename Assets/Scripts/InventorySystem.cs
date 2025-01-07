using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class ItemPrefabMapping
{
    public string itemName; 
    public GameObject itemPrefab; 
}

public class InventorySystem : MonoBehaviour
{
    public LinkedList<InventoryItem> inventory = new LinkedList<InventoryItem>();
    public int maxInventorySize = 10;
    public GameObject inventoryPanel; 
    // Panel to display inventory items
    public Text inventoryDisplayText; 
    // UI text to display inventory contents

    public List<ItemPrefabMapping> itemPrefabList; 
    // List to populate in the Inspector
    private Dictionary<string, GameObject> itemPrefabs; 
    
    // Dictionary for quick lookup

    private void Start()
    {
        // Hide the inventory panel initially
        inventoryPanel.SetActive(false);

        // Convert the itemPrefabList to a dictionary for fast access
        itemPrefabs = new Dictionary<string, GameObject>();
        foreach (var mapping in itemPrefabList)
        {
            if (!itemPrefabs.ContainsKey(mapping.itemName))
            {
                itemPrefabs.Add(mapping.itemName, mapping.itemPrefab);
            }
        }

        // Load inventory when starting the game
        LoadInventory();
    }

    private void Update()
    {
        // Toggle the inventory display when the user presses the "I" key
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventoryDisplay();
        }

        // Drop the first item in the inventory when the "Z" key is pressed
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (inventory.Count > 0)
            {
                DropItem(inventory.First.Value.itemName);
            }
            else
            {
                Debug.Log("No items in inventory to drop.");
            }
        }
    }

    public void AddItem(string itemName, int quantity)
    {
        // Check if the item already exists in the inventory
        foreach (var item in inventory)
        {
            if (item.itemName == itemName)
            {
                item.quantity += quantity;
                UpdateInventoryDisplay();
                return;
            }
        }

        // Add a new item if the inventory is not full
        if (inventory.Count < maxInventorySize)
        {
            inventory.AddLast(new InventoryItem(itemName, quantity));
            UpdateInventoryDisplay();
        }
        else
        {
            Debug.Log("Inventory is full!");
        }
    }

    public void DropItem(string itemName)
    {
        LinkedListNode<InventoryItem> currentNode = inventory.First;

        while (currentNode != null)
        {
            if (currentNode.Value.itemName == itemName)
            {
                // Decrease the quantity and remove the item if needed
                currentNode.Value.quantity--;
                if (currentNode.Value.quantity <= 0)
                {
                    inventory.Remove(currentNode);
                }
                UpdateInventoryDisplay();
                // Create a dropped item in the scene
                SpawnDroppedItem(itemName);
                return;
            }
            currentNode = currentNode.Next;
        }

        Debug.Log("Item not found in inventory.");
    }

    private void SpawnDroppedItem(string itemName)
    {
        if (itemPrefabs.ContainsKey(itemName))
        {
            GameObject itemPrefab = itemPrefabs[itemName];
            if (itemPrefab != null)
            {
                // Get player's current position and forward direction
                Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
                Vector3 spawnPosition = playerTransform.position + playerTransform.forward * 2; 

                // Instantiate the item prefab at the calculated position
                Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
                Debug.Log("Dropped " + itemName + " in front of the player.");
            }
        }
        else
        {
            Debug.Log("No prefab found for item: " + itemName);
        }
    }

    public void SaveInventory()
    {
        string path = Application.persistentDataPath + "/inventory.txt";
        using (StreamWriter writer = new StreamWriter(path))
        {
            foreach (var item in inventory)
            {
                writer.WriteLine(item.itemName + "," + item.quantity);
            }
        }
        Debug.Log("Inventory saved to " + path);
    }

    public void LoadInventory()
    {
        string path = Application.persistentDataPath + "/inventory.txt";
        if (File.Exists(path))
        {
            inventory.Clear();
            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] data = line.Split(',');
                    string itemName = data[0];
                    int quantity = int.Parse(data[1]);
                    inventory.AddLast(new InventoryItem(itemName, quantity));
                }
            }
            Debug.Log("Inventory loaded from " + path);
            UpdateInventoryDisplay();
        }
        else
        {
            Debug.Log("No previous inventory file found. Starting a new game.");
        }
    }

    public void UpdateInventoryDisplay()
    {
        if (inventoryDisplayText != null)
        {
            inventoryDisplayText.text = "Inventory:\n";
            foreach (var item in inventory)
            {
                inventoryDisplayText.text += item.itemName + " x" + item.quantity + "\n";
            }
        }
    }

    private void ToggleInventoryDisplay()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    private void OnApplicationQuit()
    {
        SaveInventory();
    }
}
