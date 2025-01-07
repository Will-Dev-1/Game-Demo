using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject settingsMenu;

    private bool isMenuOpen = false;

    void Update()
    {
        // Check for ESC key press to open/close the menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        settingsMenu.SetActive(isMenuOpen);

        if (isMenuOpen)
        {
            // Unlock and show the cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            // Lock and hide the cursor for first-person control
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f; // Resume the game
        }
    }
}
