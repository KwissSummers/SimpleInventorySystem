using UnityEngine;

public class SimpleToggle : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject mainHUDPanel;

    public void Toggle()
    {
        if (inventoryPanel != null && mainHUDPanel != null)
        {
            // Force the states we want - don't toggle
            inventoryPanel.SetActive(true);
            mainHUDPanel.SetActive(false);

            // Start a coroutine to ensure the panel stays visible
            StartCoroutine(EnsurePanelVisibility());
            Debug.Log("Direct toggle called with persistent visibility");
        }
    }

    private System.Collections.IEnumerator EnsurePanelVisibility()
    {
        // Check every frame for a short time to make sure the panel stays visible
        for (int i = 0; i < 10; i++)
        {
            // Force the panel to stay visible
            if (!inventoryPanel.activeSelf)
            {
                inventoryPanel.SetActive(true);
                mainHUDPanel.SetActive(false);
                Debug.Log("Corrected panel visibility");
            }
            yield return null;
        }
    }
}