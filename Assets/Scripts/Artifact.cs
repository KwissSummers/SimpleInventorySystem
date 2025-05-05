using UnityEngine;

[System.Serializable]
public class Artifact : Item
{
    public int defenseBonus;
    public ArtifactType artifactType;

    public enum ArtifactType
    {
        Helmet,
        Chestpiece,
        Gloves,
        Boots,
        Accessory
    }

    public Artifact(string id, string name, string description, ItemRarity rarity,
                   ArtifactType type, int defenseBonus, Sprite icon = null)
        : base(id, name, description, rarity, ItemCategory.Artifact, icon)
    {
        this.defenseBonus = defenseBonus;
        this.artifactType = type;
        this.maxStackSize = 1; // Artifacts typically can't be stacked
    }

    public override void Use()
    {
        Debug.Log($"Equipped artifact: {itemName} - Defense Bonus: {defenseBonus}");
        // Code for equipping the artifact
    }

    public override Item Clone()
    {
        Artifact newArtifact = new Artifact(id, itemName, description, rarity, artifactType, defenseBonus, icon);
        newArtifact.currentStackSize = this.currentStackSize;
        return newArtifact;
    }
}