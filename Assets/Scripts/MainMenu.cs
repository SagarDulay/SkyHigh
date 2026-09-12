// SOLID — Single Responsibility:
// MainMenu only handles main menu input and scene loading.
// Cursor state is unlocked here to ensure clean UI interaction
// regardless of what state the previous scene left it in.

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (AudioManager.Instance != null)
            AudioManager.Instance.StartMenuAmbient();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}