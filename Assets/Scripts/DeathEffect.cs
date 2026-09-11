using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    public static DeathEffect Instance;

    [SerializeField] private Animator flashAnimator;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayDeathFlash()
    {
        flashAnimator.SetTrigger("Flash");
    }
}