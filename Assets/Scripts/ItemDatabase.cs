using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance { get; private set; }

    // Lists of item prefabs
    public List<Sprite> itemIcons = new List<Sprite>();
    private Dictionary<string, Item> itemTemplates = new Dictionary<string, Item>();

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Load icons
        LoadIcons();

        // Initialize the database with sample items
        InitializeDatabase();
    }

    private void LoadIcons()
    {
        // Load all sprites from Resources/Icons folder
        Sprite[] loadedIcons = Resources.LoadAll<Sprite>("Icons");
        foreach (Sprite icon in loadedIcons)
        {
            itemIcons.Add(icon);
        }
    }

    private void InitializeDatabase()
    {
        // Add sample items to the database

        // Weapons
        AddItem(new Weapon("w001", "Iron Sword", "A basic sword made of iron.",
                         Item.ItemRarity.Common, Weapon.WeaponType.Sword, 10, GetRandomIcon()));

        AddItem(new Weapon("w002", "Enchanted Bow", "A bow imbued with magical energy.",
                         Item.ItemRarity.Rare, Weapon.WeaponType.Bow, 25, GetRandomIcon()));

        AddItem(new Weapon("w003", "Dragon Slayer", "A legendary claymore said to have slain dragons.",
                         Item.ItemRarity.Legendary, Weapon.WeaponType.Claymore, 50, GetRandomIcon()));

        // Artifacts
        AddItem(new Artifact("a001", "Leather Helmet", "Basic protection for your head.",
                           Item.ItemRarity.Common, Artifact.ArtifactType.Helmet, 5, GetRandomIcon()));

        AddItem(new Artifact("a002", "Silver Gauntlets", "Reinforced gauntlets made of silver.",
                           Item.ItemRarity.Uncommon, Artifact.ArtifactType.Gloves, 12, GetRandomIcon()));

        AddItem(new Artifact("a003", "Celestial Armor", "Armor forged from star metal.",
                           Item.ItemRarity.Epic, Artifact.ArtifactType.Chestpiece, 30, GetRandomIcon()));

        // Consumables
        AddItem(new Consumable("c001", "Minor Health Potion", "Restores a small amount of health.",
                             Item.ItemRarity.Common, 20, 0, GetRandomIcon()));

        AddItem(new Consumable("c002", "Energy Drink", "Restores energy.",
                             Item.ItemRarity.Uncommon, 0, 30, GetRandomIcon()));

        AddItem(new Consumable("c003", "Elixir of Vitality", "Completely restores health and energy.",
                             Item.ItemRarity.Epic, 100, 100, GetRandomIcon()));

        // Materials
        AddItem(new Material("m001", "Iron Ore", "Raw iron that can be refined.",
                           Item.ItemRarity.Common, Material.MaterialType.Ore, GetRandomIcon()));

        AddItem(new Material("m002", "Healing Herb", "A plant with medicinal properties.",
                           Item.ItemRarity.Common, Material.MaterialType.Plant, GetRandomIcon()));

        AddItem(new Material("m003", "Dragon Scale", "A scale from a powerful dragon.",
                           Item.ItemRarity.Rare, Material.MaterialType.Monster, GetRandomIcon()));
    }

    private Sprite GetRandomIcon()
    {
        // Return a random icon from our collection, or null if we have none
        if (itemIcons.Count > 0)
        {
            return itemIcons[Random.Range(0, itemIcons.Count)];
        }
        return null;
    }

    private void AddItem(Item item)
    {
        if (!itemTemplates.ContainsKey(item.id))
        {
            itemTemplates.Add(item.id, item);
        }
    }

    public Item GetItemById(string id)
    {
        if (itemTemplates.TryGetValue(id, out Item item))
        {
            return item.Clone();
        }
        return null;
    }

    public List<Item> GetAllItems()
    {
        List<Item> result = new List<Item>();
        foreach (var item in itemTemplates.Values)
        {
            result.Add(item.Clone());
        }
        return result;
    }
}