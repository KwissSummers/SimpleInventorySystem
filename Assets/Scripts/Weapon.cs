using UnityEngine;

[System.Serializable]
public class Weapon : Item
{
    public int attackPower;
    public WeaponType weaponType;

    public enum WeaponType
    {
        Sword,
        Bow,
        Polearm,
        Catalyst,
        Claymore
    }

    public Weapon(string id, string name, string description, ItemRarity rarity,
                 WeaponType type, int attackPower, Sprite icon = null)
        : base(id, name, description, rarity, ItemCategory.Weapon, icon)
    {
        this.attackPower = attackPower;
        this.weaponType = type;
        this.maxStackSize = 1; // Weapons typically can't be stacked
    }

    public override void Use()
    {
        Debug.Log($"Equipped weapon: {itemName} - Attack Power: {attackPower}");
        // Code for equipping the weapon
    }

    public override Item Clone()
    {
        Weapon newWeapon = new Weapon(id, itemName, description, rarity, weaponType, attackPower, icon);
        newWeapon.currentStackSize = this.currentStackSize;
        return newWeapon;
    }
}