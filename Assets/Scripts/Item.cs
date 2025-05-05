using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string id;
    public string itemName;
    public string description;
    public Sprite icon;
    public ItemRarity rarity;
    public ItemCategory category;
    public int maxStackSize = 9999;
    public int currentStackSize = 1;

    public enum ItemRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    public enum ItemCategory
    {
        Weapon,
        Artifact,
        Consumable,
        Material
    }

    public Item(string id, string name, string description, ItemRarity rarity, ItemCategory category, Sprite icon = null)
    {
        this.id = id;
        this.itemName = name;
        this.description = description;
        this.rarity = rarity;
        this.category = category;
        this.icon = icon;
    }

    public virtual void Use()
    {
        Debug.Log($"Using item: {itemName}");
        // Base functionality for using an item
    }

    // Clone method for creating a copy of an item
    public virtual Item Clone()
    {
        return new Item(id, itemName, description, rarity, category, icon)
        {
            maxStackSize = this.maxStackSize,
            currentStackSize = this.currentStackSize
        };
    }
}