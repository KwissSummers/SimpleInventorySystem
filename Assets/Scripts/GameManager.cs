using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button addRandomItemButton;

    private void Start()
    {
        // Set up button listeners
        if (inventoryButton) inventoryButton.onClick.AddListener(ToggleInventory);
        if (saveButton) saveButton.onClick.AddListener(SaveInventory);
        if (loadButton) loadButton.onClick.AddListener(LoadInventory);
        if (addRandomItemButton) addRandomItemButton.onClick.AddListener(AddRandomItem);
    }

    private void ToggleInventory()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ToggleInventory();
        }
    }

    private void SaveInventory()
    {
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.SaveInventory();
            Debug.Log("Inventory saved");
        }
    }

    private void LoadInventory()
    {
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.LoadInventory();
            Debug.Log("Inventory loaded");
        }
    }

    private void AddRandomItem()
    {
        if (ItemDatabase.Instance != null && Inventory.Instance != null)
        {
            // Get all available items
            List<Item> allItems = ItemDatabase.Instance.GetAllItems();

            // Select a random item
            if (allItems.Count > 0)
            {
                Item randomItem = allItems[Random.Range(0, allItems.Count)];

                // Add to inventory
                bool success = Inventory.Instance.AddItem(randomItem);

                // Show feedback
                Debug.Log(success ? $"Added {randomItem.itemName} to inventory" : "Failed to add item (inventory might be full)");
            }
            else
            {
                Debug.LogWarning("No items found in the database");
            }
        }
    }
}