using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Inventory UI")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private Image itemIconImage;
    [SerializeField] private Button useItemButton;
    [SerializeField] private Button dropItemButton;

    [Header("Filter Buttons")]
    [SerializeField] private Button allCategoryButton;
    [SerializeField] private Button weaponsButton;
    [SerializeField] private Button artifactsButton;
    [SerializeField] private Button consumablesButton;
    [SerializeField] private Button materialsButton;

    [Header("Sort Buttons")]
    [SerializeField] private Button sortByNameButton;
    [SerializeField] private Button sortByRarityButton;

    [Header("HUD References")]
    [SerializeField] private GameObject mainHUDPanel; // Add this reference

    private Item selectedItem;
    private List<GameObject> instantiatedSlots = new List<GameObject>();
    private Item.ItemCategory currentCategoryFilter = Item.ItemCategory.Weapon; // Default filter
    private bool isFilterActive = false;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Hide details panel by default
        ClearItemDetails();
    }

    private void Start()
    {
        // Find the MainHUDPanel if not assigned
        if (mainHUDPanel == null)
        {
            mainHUDPanel = GameObject.Find("MainHUDPanel");
        }

        // Subscribe to inventory changes
        if (Inventory.Instance != null)
        {
            Inventory.Instance.OnInventoryChanged += RefreshInventoryUI;
        }
        else
        {
            Debug.LogError("Inventory instance not found!");
        }

        // Set up button listeners
        if (useItemButton) useItemButton.onClick.AddListener(UseSelectedItem);
        if (dropItemButton) dropItemButton.onClick.AddListener(DropSelectedItem);

        // Set up category filter buttons
        if (allCategoryButton) allCategoryButton.onClick.AddListener(FilterAll);
        if (weaponsButton) weaponsButton.onClick.AddListener(FilterWeapons);
        if (artifactsButton) artifactsButton.onClick.AddListener(FilterArtifacts);
        if (consumablesButton) consumablesButton.onClick.AddListener(FilterConsumables);
        if (materialsButton) materialsButton.onClick.AddListener(FilterMaterials);

        // Set up sort buttons
        if (sortByNameButton) sortByNameButton.onClick.AddListener(() => SortByName());
        if (sortByRarityButton) sortByRarityButton.onClick.AddListener(() => SortByRarity());

        // Make sure inventory panel is off at start
        if (inventoryPanel)
        {
            inventoryPanel.SetActive(false);
        }

        // Initial UI refresh
        RefreshInventoryUI();
    }

    // Public methods that can be called from buttons
    public void ToggleInventory()
    {
        if (inventoryPanel)
        {
            bool newState = !inventoryPanel.activeSelf;
            inventoryPanel.SetActive(newState);

            // Toggle the HUD visibility (opposite of inventory)
            if (mainHUDPanel)
            {
                mainHUDPanel.SetActive(!newState);
            }

            if (inventoryPanel.activeSelf)
            {
                RefreshInventoryUI();
            }
        }
    }
    // Replace the single SetCategoryFilter method with individual methods for each category
    public void FilterAll()
    {
        isFilterActive = false;
        RefreshInventoryUI();
    }

    public void FilterWeapons()
    {
        currentCategoryFilter = Item.ItemCategory.Weapon;
        isFilterActive = true;
        RefreshInventoryUI();
    }

    public void FilterArtifacts()
    {
        currentCategoryFilter = Item.ItemCategory.Artifact;
        isFilterActive = true;
        RefreshInventoryUI();
    }

    public void FilterConsumables()
    {
        currentCategoryFilter = Item.ItemCategory.Consumable;
        isFilterActive = true;
        RefreshInventoryUI();
    }

    public void FilterMaterials()
    {
        currentCategoryFilter = Item.ItemCategory.Material;
        isFilterActive = true;
        RefreshInventoryUI();
    }

    public void SortByName()
    {
        if (Inventory.Instance != null)
        {
            Inventory.Instance.SortItemsByName();
        }
    }

    public void SortByRarity()
    {
        if (Inventory.Instance != null)
        {
            Inventory.Instance.SortItemsByRarity(false);
        }
    }

    private void RefreshInventoryUI()
    {
        if (itemsContainer == null || itemSlotPrefab == null)
        {
            Debug.LogError("Items container or item slot prefab is not assigned!");
            return;
        }

        // Clear existing slots
        foreach (GameObject slot in instantiatedSlots)
        {
            Destroy(slot);
        }
        instantiatedSlots.Clear();

        if (Inventory.Instance == null)
        {
            Debug.LogError("Inventory instance not found!");
            return;
        }

        // Get items based on filter
        List<Item> itemsToDisplay;
        if (isFilterActive)
        {
            itemsToDisplay = Inventory.Instance.GetItemsByCategory(currentCategoryFilter);
        }
        else
        {
            itemsToDisplay = Inventory.Instance.GetItems();
        }

        // Create new slots for each item
        foreach (Item item in itemsToDisplay)
        {
            GameObject slotGO = Instantiate(itemSlotPrefab, itemsContainer);
            instantiatedSlots.Add(slotGO);

            // Set up slot UI
            ItemSlotUI slotUI = slotGO.GetComponent<ItemSlotUI>();
            if (slotUI != null)
            {
                slotUI.SetItem(item);
                slotUI.OnItemClicked += SelectItem;
            }
        }

        // Clear selection if the selected item is no longer in inventory
        if (selectedItem != null && !itemsToDisplay.Contains(selectedItem))
        {
            ClearItemDetails();
        }
    }

    private void SelectItem(Item item)
    {
        selectedItem = item;

        // Update item details UI
        if (itemNameText) itemNameText.text = item.itemName;
        if (itemDescriptionText) itemDescriptionText.text = item.description;
        if (itemIconImage && item.icon) itemIconImage.sprite = item.icon;

        // Show item details panel
        useItemButton.gameObject.SetActive(true);
        dropItemButton.gameObject.SetActive(true);

        // Enable/disable use button based on item type
        useItemButton.interactable = (item.category == Item.ItemCategory.Weapon ||
                                     item.category == Item.ItemCategory.Artifact ||
                                     item.category == Item.ItemCategory.Consumable);
    }

    private void ClearItemDetails()
    {
        selectedItem = null;

        // Clear UI elements
        if (itemNameText) itemNameText.text = "";
        if (itemDescriptionText) itemDescriptionText.text = "";
        if (itemIconImage) itemIconImage.sprite = null;

        // Hide buttons
        if (useItemButton) useItemButton.gameObject.SetActive(false);
        if (dropItemButton) dropItemButton.gameObject.SetActive(false);
    }

    public void UseSelectedItem()
    {
        if (selectedItem != null && Inventory.Instance != null)
        {
            Inventory.Instance.UseItem(selectedItem);

            // If the item was completely used up, clear selection
            if (selectedItem.currentStackSize <= 0)
            {
                ClearItemDetails();
            }
        }
    }

    public void DropSelectedItem()
    {
        if (selectedItem != null && Inventory.Instance != null)
        {
            Inventory.Instance.RemoveItem(selectedItem);
            ClearItemDetails();
        }
    }
}