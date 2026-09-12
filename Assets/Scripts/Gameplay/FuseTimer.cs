// DESIGN PATTERN: Strategy (IButtonReaction)
// FuseTimer defines its own timed reaction to a button press.
// PuzzleButton fires its UnityEvent — FuseTimer decides what that means.
// When the fuse completes it fires its own UnityEvent, chaining reactions
// without any object knowing what comes next.
//
// SOLID — Single Responsibility:
// FuseTimer only handles countdown logic and completion. Nothing else.

using UnityEngine;
using UnityEngine.Events;

public class FuseTimer : MonoBehaviour, IButtonReaction, IResettable
{
    [Header("Settings")]
    [SerializeField] private float fuseLength = 3f;

    [Header("On Fuse Complete")]
    public UnityEvent onFuseComplete;

    private bool _isRunning = false;
    private float _timer = 0f;

    // IButtonReaction — called by PuzzleButton's UnityEvent
    public void OnButtonActivated()
    {
        if (_isRunning) return;
        _isRunning = true;
        _timer = fuseLength;
    }

    private void Update()
    {
        if (!_isRunning) return;

        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            _isRunning = false;
            onFuseComplete.Invoke();
        }
    }

    // IResettable — called by CheckpointManager on player death
    public void ResetState()
    {
        _isRunning = false;
        _timer = 0f;
    }
}