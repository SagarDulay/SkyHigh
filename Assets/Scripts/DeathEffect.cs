// SOLID — Single Responsibility:
// DeathEffect only handles the visual death flash animation.
// It triggers the Animator and nothing else.
//
// DESIGN PATTERN: Singleton
// One instance handles all death flash requests from anywhere in the scene.

using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    public static DeathEffect Instance;

    [SerializeField] private Animator flashAnimator;

    private void Awake()
    {
        Instance = this;
    }

    // Triggers the Flash animation via the Animator Controller
    public void PlayDeathFlash()
    {
        flashAnimator.SetTrigger("Flash");
    }
}