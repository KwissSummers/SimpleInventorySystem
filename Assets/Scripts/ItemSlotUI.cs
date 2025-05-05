using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI stackSizeText;
    [SerializeField] private Image rarityBorder;

    [Header("Rarity Colors")]
    [SerializeField] private Color commonColor = Color.gray;
    [SerializeField] private Color uncommonColor = Color.green;
    [SerializeField] private Color rareColor = Color.blue;
    [SerializeField] private Color epicColor = Color.magenta;
    [SerializeField] private Color legendaryColor = Color.yellow;

    private Item item;

    public event Action<Item> OnItemClicked;

    public void SetItem(Item newItem)
    {
        item = newItem;

        // Update UI elements
        if (iconImage && newItem.icon) iconImage.sprite = newItem.icon;
        if (itemNameText) itemNameText.text = newItem.itemName;

        // Show stack size if more than 1
        if (stackSizeText)
        {
            stackSizeText.gameObject.SetActive(newItem.currentStackSize > 1);
            stackSizeText.text = newItem.currentStackSize.ToString();
        }

        // Set rarity border color
        if (rarityBorder)
        {
            switch (newItem.rarity)
            {
                case Item.ItemRarity.Common:
                    rarityBorder.color = commonColor;
                    break;
                case Item.ItemRarity.Uncommon:
                    rarityBorder.color = uncommonColor;
                    break;
                case Item.ItemRarity.Rare:
                    rarityBorder.color = rareColor;
                    break;
                case Item.ItemRarity.Epic:
                    rarityBorder.color = epicColor;
                    break;
                case Item.ItemRarity.Legendary:
                    rarityBorder.color = legendaryColor;
                    break;
            }
        }
    }

    public void OnClick()
    {
        if (item != null)
        {
            OnItemClicked?.Invoke(item);
        }
    }
}