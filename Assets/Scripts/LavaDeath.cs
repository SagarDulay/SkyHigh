// SOLID — Single Responsibility:
// LavaDeath is a marker component only.
// It exists solely so PlayerController can detect death surfaces via GetComponent.
// No logic lives here — detection is handled by PlayerController.OnControllerColliderHit.

using UnityEngine;

public class LavaDeath : MonoBehaviour { }