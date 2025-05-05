using UnityEngine;

[System.Serializable]
public class Material : Item
{
    public MaterialType materialType;

    public enum MaterialType
    {
        Ore,
        Plant,
        Monster,
        Artifact,
        Special
    }

    public Material(string id, string name, string description, ItemRarity rarity,
                   MaterialType type, Sprite icon = null)
        : base(id, name, description, rarity, ItemCategory.Material, icon)
    {
        this.materialType = type;
        this.maxStackSize = 9999; // Materials can be stacked to a high amount
    }

    public override Item Clone()
    {
        Material newMaterial = new Material(id, itemName, description, rarity, materialType, icon);
        newMaterial.currentStackSize = this.currentStackSize;
        return newMaterial;
    }
}