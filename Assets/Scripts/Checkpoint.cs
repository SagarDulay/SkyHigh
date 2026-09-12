// SOLID — Single Responsibility:
// Checkpoint only handles setting the current checkpoint position.
// It notifies CheckpointManager when the player walks through it.
// No respawn logic lives here — that belongs to CheckpointManager.

using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CheckpointManager.Instance.SetCheckpoint(transform.position);
        }
    }
}