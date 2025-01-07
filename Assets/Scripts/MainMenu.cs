using UnityEngine;
using UnityEngine.SceneManagement; 
using System.IO;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel; 
    public InventorySystem inventorySystem; 
    public GameSettings gameSettings; 
    public MenuController menuController; 

    private void Start()
    {
        
        if (SceneManager.GetActiveScene().buildIndex == 0) 
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            mainMenuPanel.SetActive(true);
            Time.timeScale = 0f; 
            // Pause the game until an option is chosen

            // Disable the MenuController initially
            if (menuController != null)
            {
                menuController.enabled = false;
            }
        }
        else
        {
            // Hide the main menu in subsequent levels
            mainMenuPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f; // Resume the game
        }
    }

    public void StartNewGame()
    {
        Debug.Log("StartNewGame button clicked.");
        ClearSavedData();
        gameSettings.LoadDefaultSettings();
        inventorySystem.inventory.Clear();
        inventorySystem.UpdateInventoryDisplay();

        mainMenuPanel.SetActive(false);
        ResumeGameplay();

        // Enable MenuController after start
        if (menuController != null)
        {
            menuController.enabled = true;
        }

        Debug.Log("Starting a new game...");
    }

    public void ResumeGame()
    {
        Debug.Log("ResumeGame button clicked.");
        gameSettings.LoadSettings();
        inventorySystem.LoadInventory();

        mainMenuPanel.SetActive(false);
        ResumeGameplay();

        // Enable MenuController after resuming
        if (menuController != null)
        {
            menuController.enabled = true;
        }

        Debug.Log("Resuming the last saved game...");
    }

    private void ResumeGameplay()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor for gameplay
        Cursor.visible = false; // Hide the cursor for gameplay
        Time.timeScale = 1f; // Resume the game
    }

    private void ClearSavedData()
    {
        string inventoryPath = Application.persistentDataPath + "/inventory.txt";
        if (File.Exists(inventoryPath))
        {
            File.Delete(inventoryPath);
        }

        string settingsPath = Application.persistentDataPath + "/gamesettings.txt";
        if (File.Exists(settingsPath))
        {
            File.Delete(settingsPath);
        }
    }
}
