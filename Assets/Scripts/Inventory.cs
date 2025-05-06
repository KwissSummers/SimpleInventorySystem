using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    [SerializeField] private int maxCapacity = 99999;
    private List<Item> items = new List<Item>();

    // Events for UI updates
    public event Action OnInventoryChanged;

    private void Awake()
    {
        // Singleton pattern
    if (Instance == null)
    {
        Instance = this;
        // Preserve the entire Systems GameObject structure
        Transform root = transform;
        while (root.parent != null)
        {
            root = root.parent;
        }
        DontDestroyOnLoad(root.gameObject);
    }
    else
    {
        Destroy(gameObject);
    }
    }

    public bool AddItem(Item item)
    {
        if (item == null)
        {
            Debug.LogError("Attempted to add null item to inventory");
            return false;
        }
        if (items.Count >= maxCapacity)
        {
            Debug.Log("Inventory is full!");
            return false;
        }

        // Try to stack the item if possible
        if (item.currentStackSize > 1)
        {
            Item existingItem = items.Find(i => i.id == item.id);
            if (existingItem != null && existingItem.currentStackSize < existingItem.maxStackSize)
            {
                int spaceInStack = existingItem.maxStackSize - existingItem.currentStackSize;
                int amountToAdd = Mathf.Min(spaceInStack, item.currentStackSize);

                existingItem.currentStackSize += amountToAdd;
                item.currentStackSize -= amountToAdd;

                // If we still have items left to add
                if (item.currentStackSize > 0)
                {
                    Item newItem = item.Clone();
                    newItem.currentStackSize = item.currentStackSize;
                    items.Add(newItem);
                }
            }
            else
            {
                items.Add(item.Clone());
            }
        }
        else
        {
            items.Add(item.Clone());
        }

        OnInventoryChanged?.Invoke();
        return true;
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
        OnInventoryChanged?.Invoke();
    }

    public List<Item> GetItems()
    {
        return items;
    }

    public List<Item> GetItemsByCategory(Item.ItemCategory category)
    {
        return items.Where(item => item.category == category).ToList();
    }

    public void SortItemsByName()
    {
        items = items.OrderBy(item => item.itemName).ToList();
        OnInventoryChanged?.Invoke();
    }

    public void SortItemsByRarity(bool ascending = false)
    {
        if (ascending)
        {
            items = items.OrderBy(item => (int)item.rarity).ToList();
        }
        else
        {
            items = items.OrderByDescending(item => (int)item.rarity).ToList();
        }
        OnInventoryChanged?.Invoke();
    }

    public void UseItem(Item item)
    {
        item.Use();

        // If the item was consumed completely, remove it
        if (item.currentStackSize <= 0)
        {
            RemoveItem(item);
        }
        else
        {
            OnInventoryChanged?.Invoke();
        }
    }

    public void ClearInventory()
    {
        items.Clear();
        OnInventoryChanged?.Invoke();
    }
}