// DESIGN PATTERN: Factory + Command
// ToyGun creates a ShootCommand (Factory) and calls Execute() (Command).
// The weapon never instantiates the projectile directly — ShootCommand handles that.
// ToyGun depends only on ICommand, never on Dart or any concrete projectile type.
//
// SOLID — Dependency Inversion:
// ToyGun depends on ICommand abstraction, not ShootCommand directly.
// Swap ShootCommand for any other ICommand without touching ToyGun.

using UnityEngine;

public class ToyGun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject dartPrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("Settings")]
    [SerializeField] private float fireRate = 0.4f;

    private float _nextFireTime;
    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        if (_cam == null) return;

        if (Input.GetButtonDown("Fire1") && Time.time >= _nextFireTime)
        {
            Fire();
            _nextFireTime = Time.time + fireRate;
        }
    }

    private void Fire()
    {
        if (_cam == null) return;
        if (dartPrefab == null) return;
        if (spawnPoint == null) return;

        Vector3 direction = _cam.transform.forward;
        ICommand shot = new ShootCommand(dartPrefab, spawnPoint, direction);
        shot.Execute();

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayDartShoot();
    }
}