// DESIGN PATTERN: Strategy (IButtonReaction)
// MovingPlatform defines its own reaction to a button press independently.
// PuzzleButton fires its UnityEvent — MovingPlatform decides what that means.
//
// SOLID — Single Responsibility:
// MovingPlatform only handles movement, waiting, and resetting. Nothing else.
//
// SOLID — Open/Closed:
// New platform behaviors can be added without modifying PuzzleButton.

using UnityEngine;

public class MovingPlatform : MonoBehaviour, IButtonReaction, IResettable
{
    [Header("Settings")]
    [SerializeField] private Transform targetPoint;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private bool returnAfterReaching = false;
    [SerializeField] private float pauseAtDestination = 0.15f;

    private Vector3 _startPosition;
    private bool _isMoving = false;
    private bool _returning = false;
    private bool _waiting = false;
    private float _waitTimer = 0f;

    private void Awake()
    {
        _startPosition = transform.position;
    }

    // IButtonReaction — called by PuzzleButton's UnityEvent
    public void OnButtonActivated()
    {
        _isMoving = true;
        _returning = false;
        _waiting = false;
    }

    private void Update()
    {
        if (!_isMoving) return;

        // Wait at destination before returning
        if (_waiting)
        {
            _waitTimer -= Time.deltaTime;
            if (_waitTimer <= 0f)
            {
                _waiting = false;
                _returning = true;
            }
            return;
        }

        Vector3 destination = _returning ? _startPosition : targetPoint.position;

        // Ease into destination — slows down as it approaches
        float distance = Vector3.Distance(transform.position, destination);
        float speed = Mathf.Clamp(distance * 2f, 0.5f, moveSpeed);

        transform.position = Vector3.MoveTowards(
            transform.position,
            destination,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, destination) < 0.01f)
        {
            transform.position = destination;

            if (returnAfterReaching && !_returning)
            {
                // Brief pause before retracting
                _waiting = true;
                _waitTimer = pauseAtDestination;
            }
            else
            {
                _isMoving = false;
            }
        }
    }

    // IResettable — called by CheckpointManager on player death
    public void ResetState()
    {
        _isMoving = false;
        _returning = false;
        _waiting = false;
        _waitTimer = 0f;
        transform.position = _startPosition;
    }
}