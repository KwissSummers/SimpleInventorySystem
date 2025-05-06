using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    private static bool isInitialized = false;
    private static GameObject systemsObject;

    void Awake()
    {
        if (!isInitialized)
        {
            InitializeSystems();
        }
    }

    private void InitializeSystems()
    {
        // Create a persistent systems object
        systemsObject = new GameObject("Systems");
        DontDestroyOnLoad(systemsObject);

        // Add required components
        systemsObject.AddComponent<Inventory>();
        systemsObject.AddComponent<ItemDatabase>();
        systemsObject.AddComponent<SaveSystem>();

        Debug.Log("Game systems initialized programmatically");
        isInitialized = true;
    }
}