// SOLID — Single Responsibility:
// CheckpointManager only handles player respawning and puzzle resets.
//
// SOLID — Open/Closed:
// ResetAllPuzzles() finds every IResettable in the scene automatically.
// Adding a new resettable object requires zero changes to this script.
//
// DESIGN PATTERN: Singleton
// One instance manages all checkpoint state for the session.

using UnityEngine;
using System.Collections;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    [Header("Settings")]
    [SerializeField] private Vector3 startPosition;

    private Vector3 _currentCheckpoint;

    private void Awake()
    {
        Instance = this;
        _currentCheckpoint = startPosition;
    }

    public void SetCheckpoint(Vector3 position)
    {
        _currentCheckpoint = position;
    }

    public void RespawnPlayer(Transform player)
    {
        // Trigger visual and audio death feedback
        DeathEffect.Instance.PlayDeathFlash();
        AudioManager.Instance.PlayDeath();

        // Reset all puzzle objects before repositioning player
        ResetAllPuzzles();

        StartCoroutine(DelayedRespawn(player));
    }

    private IEnumerator DelayedRespawn(Transform player)
    {
        // Brief delay so death flash is visible before teleport
        yield return new WaitForSeconds(0.3f);

        CharacterController cc = player.GetComponent<CharacterController>();
        cc.enabled = false;
        player.position = _currentCheckpoint;
        cc.enabled = true;
    }

    private void ResetAllPuzzles()
    {
        // SOLID — Open/Closed: finds every IResettable automatically.
        // No manual registration needed — new objects are picked up automatically.
        foreach (MonoBehaviour mb in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
        {
            if (mb is IResettable resettable)
            {
                resettable.ResetState();
            }
        }
    }
}