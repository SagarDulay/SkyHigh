// SOLID — Single Responsibility:
// PauseManager only handles pause state, timescale, cursor,
// and scene loading to the main menu. Nothing else.
//
// Pause freezes Time.timeScale to 0 — all physics and animations
// stop automatically. Resume restores it to 1 and re-locks the cursor.

using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    private bool _isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        _isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayPause();
    }

    public void Resume()
    {
        _isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}