using UnityEngine;

[System.Serializable]
public class Consumable : Item
{
    public int healthRestored;
    public int energyRestored;

    public Consumable(string id, string name, string description, ItemRarity rarity,
                     int healthRestored, int energyRestored, Sprite icon = null)
        : base(id, name, description, rarity, ItemCategory.Consumable, icon)
    {
        this.healthRestored = healthRestored;
        this.energyRestored = energyRestored;
        this.maxStackSize = 9999; // Consumables can typically be stacked
    }

    public override void Use()
    {
        Debug.Log($"Used consumable: {itemName} - Health: +{healthRestored}, Energy: +{energyRestored}");
        // Code for using the consumable
        currentStackSize--;
    }

    public override Item Clone()
    {
        Consumable newConsumable = new Consumable(id, itemName, description, rarity, healthRestored, energyRestored, icon);
        newConsumable.currentStackSize = this.currentStackSize;
        return newConsumable;
    }
}