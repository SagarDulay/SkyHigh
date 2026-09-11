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

    public void OnButtonActivated()
    {
        _isMoving = true;
        _returning = false;
        _waiting = false;
    }

    private void Update()
    {
        if (!_isMoving) return;

        
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
                
                _waiting = true;
                _waitTimer = pauseAtDestination;
            }
            else
            {
                _isMoving = false;
            }
        }
    }

    public void ResetState()
    {
        _isMoving = false;
        _returning = false;
        _waiting = false;
        _waitTimer = 0f;
        transform.position = _startPosition;
    }
}