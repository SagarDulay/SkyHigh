// DESIGN PATTERN: Command
// Each shot is encapsulated as a ShootCommand object implementing ICommand.
// ToyGun creates the command — the command handles all instantiation.
// This decouples the weapon from the projectile entirely.

using UnityEngine;

public class ShootCommand : ICommand
{
    private readonly GameObject _dartPrefab;
    private readonly Transform _spawnPoint;
    private readonly Vector3 _direction;

    public ShootCommand(GameObject dartPrefab, Transform spawnPoint, Vector3 direction)
    {
        _dartPrefab = dartPrefab;
        _spawnPoint = spawnPoint;
        _direction = direction;
    }

    public void Execute()
    {
        GameObject dart = Object.Instantiate(
            _dartPrefab,
            _spawnPoint.position,
            Quaternion.LookRotation(_direction)
        );

        if (dart.TryGetComponent<Dart>(out var dartScript))
        {
            dartScript.Launch(_direction);
        }
    }
}