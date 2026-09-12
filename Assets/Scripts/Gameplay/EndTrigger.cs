// SOLID — Single Responsibility:
// EndTrigger only handles the win condition and cutscene sequence.
// Scene loading and input are scoped to post-win state only.
//
// Known improvement: coroutine sequencing will be replaced with
// Unity Timeline / Playable Director in a future iteration.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class EndTrigger : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI subtitleText;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject menuButton;

    [Header("Timing")]
    [SerializeField] private float fadeInDuration = 1.5f;
    [SerializeField] private float textDelayTime = 1f;
    [SerializeField] private float textFadeInTime = 1f;

    private bool _triggered = false;
    private bool _sequenceComplete = false;
    private GameObject _player;

    private void Update()
    {
        if (!_sequenceComplete) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered) return;
        if (!other.CompareTag("Player")) return;

        _triggered = true;
        _player = other.gameObject;
        StartCoroutine(WinSequence());
    }

    private IEnumerator WinSequence()
    {
        // Lock player input immediately
        _player.GetComponent<PlayerController>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayWin();

        // Hide buttons until sequence finishes
        restartButton.SetActive(false);
        menuButton.SetActive(false);

        // Initialize canvas at full transparency
        winCanvas.SetActive(true);
        SetAlpha(backgroundImage, 0f);
        SetTextAlpha(winText, 0f);
        SetTextAlpha(subtitleText, 0f);

        // Fade background to black
        yield return StartCoroutine(FadeImage(backgroundImage, 0f, 1f, fadeInDuration));

        // Pause before text appears
        yield return new WaitForSeconds(textDelayTime);

        // Fade in win text
        yield return StartCoroutine(FadeText(winText, 0f, 1f, textFadeInTime));

        // Fade in subtitle
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(FadeText(subtitleText, 0f, 1f, textFadeInTime));

        // Show input options and unlock post-win input
        yield return new WaitForSeconds(0.5f);
        restartButton.SetActive(true);
        menuButton.SetActive(true);
        _sequenceComplete = true;
    }

    private IEnumerator FadeImage(Image img, float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            SetAlpha(img, Mathf.Lerp(from, to, elapsed / duration));
            yield return null;
        }
        SetAlpha(img, to);
    }

    private IEnumerator FadeText(TextMeshProUGUI tmp, float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            SetTextAlpha(tmp, Mathf.Lerp(from, to, elapsed / duration));
            yield return null;
        }
        SetTextAlpha(tmp, to);
    }

    private void SetAlpha(Image img, float alpha)
    {
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }

    private void SetTextAlpha(TextMeshProUGUI tmp, float alpha)
    {
        Color c = tmp.color;
        c.a = alpha;
        tmp.color = c;
    }
}