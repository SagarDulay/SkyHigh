// SOLID — Dependency Inversion:
// Dart never references Door, MovingPlatform, or any concrete type.
// It only calls IActivatable.Activate() — depends on abstraction, never implementation.
// The projectile never knows what it hit.

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Dart : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 30f;
    [SerializeField] private float lifetime = 5f;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    // Called by ShootCommand immediately after Instantiate
    public void Launch(Vector3 direction)
    {
        _rb.linearVelocity = direction.normalized * speed;
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<IActivatable>(out var target))
        {
            // Only plays when hitting an actual activatable target
            target.Activate();
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayBullseyeHit();
        }
        else
        {
            // Hit a wall or non-interactive object — no sound
        }

        Destroy(gameObject);
    }
}