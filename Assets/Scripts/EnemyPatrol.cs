// SOLID — Single Responsibility:
// EnemyPatrol only handles patrol movement and player death on contact.
//
// SOLID — Open/Closed:
// Implements IResettable so CheckpointManager resets it automatically on death.
// No changes to CheckpointManager needed when new enemy types are added.

using UnityEngine;

public class EnemyPatrol : MonoBehaviour, IResettable
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float waitTime = 0.5f;

    private Vector3 _startPosition;
    private Vector3 _target;
    private bool _waiting = false;
    private float _waitTimer = 0f;

    private void Start()
    {
        _startPosition = transform.position;
        _target = pointB.position;
    }

    private void Update()
    {
        if (_waiting)
        {
            _waitTimer -= Time.deltaTime;
            if (_waitTimer <= 0f)
                _waiting = false;
            return;
        }

        // Move toward current target
        transform.position = Vector3.MoveTowards(
            transform.position,
            _target,
            moveSpeed * Time.deltaTime
        );

        // Face direction of movement
        Vector3 direction = (_target - transform.position).normalized;
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);

        // Reached target — switch direction and wait
        if (Vector3.Distance(transform.position, _target) < 0.05f)
        {
            _target = _target == pointB.position ? pointA.position : pointB.position;
            _waiting = true;
            _waitTimer = waitTime;
        }
    }

    // Kill player on contact
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayEnemyDeath();

            CheckpointManager.Instance.RespawnPlayer(other.transform);
        }
    }

    // IResettable — called by CheckpointManager on player death
    public void ResetState()
    {
        transform.position = _startPosition;
        _target = pointB.position;
        _waiting = false;
        _waitTimer = 0f;
    }
}