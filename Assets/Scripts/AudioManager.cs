// SOLID — Single Responsibility:
// AudioManager only handles sound playback. Nothing else.
// All audio clips are assigned via the Inspector and played through
// two dedicated AudioSource components — one for SFX, one for ambient.
//
// DESIGN PATTERN: Singleton
// One instance persists across the session. Duplicate instances
// are destroyed immediately in Awake to prevent audio doubling.

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Weapon")]
    [SerializeField] private AudioClip dartShoot;
    [SerializeField] private AudioClip bullseyeHit;

    [Header("Player")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landingSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip enemyDeathSound;

    [Header("Environment")]
    [SerializeField] private AudioClip doorOpen;
    [SerializeField] private AudioClip buttonPress;

    [Header("UI")]
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip pauseSound;

    [Header("Ambient")]
    [SerializeField] private AudioClip skyAmbient;
    [SerializeField] private AudioClip menuAmbient;

    private AudioSource _sfxSource;
    private AudioSource _ambientSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _sfxSource = gameObject.AddComponent<AudioSource>();
        _sfxSource.playOnAwake = false;

        _ambientSource = gameObject.AddComponent<AudioSource>();
        _ambientSource.loop = true;
        _ambientSource.volume = 0.3f;
        _ambientSource.playOnAwake = false;
    }

    // Call this when entering Level1
    public void StartLevelAmbient()
    {
        _ambientSource.clip = skyAmbient;
        _ambientSource.Play();
    }

    // Call this when entering MainMenu
    public void StartMenuAmbient()
    {
        _ambientSource.clip = menuAmbient;
        _ambientSource.Play();
    }

    public void PlayDartShoot() => _sfxSource.PlayOneShot(dartShoot);
    public void PlayBullseyeHit() => _sfxSource.PlayOneShot(bullseyeHit, 3f);
    public void PlayJump() => _sfxSource.PlayOneShot(jumpSound);
    public void PlayLanding() => _sfxSource.PlayOneShot(landingSound);
    public void PlayDeath() => _sfxSource.PlayOneShot(deathSound, 0.1f);
    public void PlayEnemyDeath() => _sfxSource.PlayOneShot(enemyDeathSound);
    public void PlayDoorOpen() => _sfxSource.PlayOneShot(doorOpen);
    public void PlayButtonPress() => _sfxSource.PlayOneShot(buttonPress);
    public void PlayWin() => _sfxSource.PlayOneShot(winSound);
    public void PlayPause() => _sfxSource.PlayOneShot(pauseSound);
}