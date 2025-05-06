using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }

    private string saveFileName = "inventory_save.dat";

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

    public void SaveInventory()
    {
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Path.Combine(Application.persistentDataPath, saveFileName);
            FileStream stream = new FileStream(path, FileMode.Create);

            // Convert inventory to serializable format
            List<ItemData> itemDataList = new List<ItemData>();

            foreach (Item item in Inventory.Instance.GetItems())
            {
                ItemData itemData = new ItemData
                {
                    id = item.id,
                    stackSize = item.currentStackSize
                };
                itemDataList.Add(itemData);
            }

            // Save data
            SaveData data = new SaveData
            {
                items = itemDataList.ToArray()
            };

            formatter.Serialize(stream, data);
            stream.Close();

            Debug.Log("Inventory saved successfully!");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save inventory: {e.Message}");
        }
    }

    public void LoadInventory()
    {
        string path = Path.Combine(Application.persistentDataPath, saveFileName);

        if (File.Exists(path))
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                SaveData data = formatter.Deserialize(stream) as SaveData;
                stream.Close();

                if (data != null)
                {
                    // Check if Inventory.Instance exists before using it
                    if (Inventory.Instance != null)
                    {
                        // Clear current inventory
                        Inventory.Instance.ClearInventory();

                        // Check if ItemDatabase.Instance exists before using it
                        if (ItemDatabase.Instance != null)
                        {
                            // Load items
                            foreach (ItemData itemData in data.items)
                            {
                                Item item = ItemDatabase.Instance.GetItemById(itemData.id);
                                if (item != null)
                                {
                                    item.currentStackSize = itemData.stackSize;
                                    Inventory.Instance.AddItem(item);
                                }
                            }

                            Debug.Log("Inventory loaded successfully!");
                        }
                        else
                        {
                            Debug.LogError("Failed to load inventory: ItemDatabase.Instance is null");
                        }
                    }
                    else
                    {
                        Debug.LogError("Failed to load inventory: Inventory.Instance is null");
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load inventory: {e.Message}");
            }
        }
        else
        {
            Debug.Log("No save file found.");
        }
    }
}

    // Serializable classes for save data
    [System.Serializable]
    public class SaveData
    {
        public ItemData[] items;
    }

    [System.Serializable]
    public class ItemData
    {
        public string id;
        public int stackSize;
    }
