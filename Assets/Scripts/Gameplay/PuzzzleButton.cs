// DESIGN PATTERN: Observer (Subject)
// PuzzleButton is the Subject in the Observer pattern.
// It raises a UnityEvent when activated — Door, MovingPlatform, and FuseTimer
// subscribe as Observers via the Inspector.
// The button never knows what it is notifying — zero coupling between cause and effect.
//
// SOLID — Single Responsibility:
// PuzzleButton only handles activation logic and visual feedback. Nothing else.
//
// SOLID — Open/Closed:
// New reactions can be added by subscribing to onActivated in the Inspector.
// PuzzleButton never needs to be modified to support new behavior.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleButton : MonoBehaviour, IActivatable, IResettable
{
    [Header("Visuals")]
    [SerializeField] private Color pressedColor = Color.green;

    [Header("Settings")]
    [SerializeField] private bool oneShot = true;

    [Header("On Activated — wire reactions here (Observer pattern)")]
    public UnityEvent onActivated;

    private bool _hasBeenPressed = false;
    private Renderer[] _renderers;
    private Color _originalColor;

    private void Awake()
    {
        // Only grab renderers tagged as Colorable
        // White rings stay white — only red rings change color
        List<Renderer> colorableRenderers = new List<Renderer>();
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            if (r.CompareTag("Colorable"))
                colorableRenderers.Add(r);
        }
        _renderers = colorableRenderers.ToArray();

        if (_renderers.Length > 0)
            _originalColor = _renderers[0].material.color;
    }

    // IActivatable — called by Dart on collision
    public void Activate()
    {
        if (oneShot && _hasBeenPressed) return;

        _hasBeenPressed = true;

        // Flash all renderers green — entire bullseye changes color at once
        foreach (Renderer r in _renderers)
            r.material.color = pressedColor;

        onActivated.Invoke();
        AudioManager.Instance.PlayButtonPress();
    }

    // IResettable — called by CheckpointManager on player death
    public void ResetState()
    {
        _hasBeenPressed = false;

        // Restore all renderers to original color
        foreach (Renderer r in _renderers)
            r.material.color = _originalColor;
    }
}