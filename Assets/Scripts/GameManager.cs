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

    // In GameManager.cs
    private static List<Item> cachedItems;

    private void AddRandomItem()
    {
        try
        {
            // Initialize cache if needed
            if (cachedItems == null || cachedItems.Count == 0)
            {
                if (ItemDatabase.Instance != null)
                {
                    cachedItems = ItemDatabase.Instance.GetAllItems();
                    Debug.Log($"Cached {cachedItems.Count} items for future use");
                }
                else
                {
                    Debug.LogError("ItemDatabase.Instance is null, cannot cache items");
                    return;
                }
            }

            // Use cached items instead of calling ItemDatabase.Instance again
            if (cachedItems.Count > 0 && Inventory.Instance != null)
            {
                Item randomItem = cachedItems[Random.Range(0, cachedItems.Count)];
                bool success = Inventory.Instance.AddItem(randomItem);
                Debug.Log(success ? $"Added {randomItem.itemName} to inventory" : "Failed to add item");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in AddRandomItem: {e.Message}");
        }
    }

    private IEnumerator AddRandomItemCoroutine()
    {
        // Wait for the next frame to avoid potential timing issues
        yield return null;

        try
        {
            if (ItemDatabase.Instance == null)
            {
                Debug.LogError("ItemDatabase.Instance is null");
                yield break;
            }

            if (Inventory.Instance == null)
            {
                Debug.LogError("Inventory.Instance is null");
                yield break;
            }

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
        catch (System.Exception e)
        {
            Debug.LogError($"Error adding item: {e.Message}\n{e.StackTrace}");
        }
    }
}