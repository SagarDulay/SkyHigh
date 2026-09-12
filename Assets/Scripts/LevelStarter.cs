using UnityEngine;

public class LevelStarter : MonoBehaviour
{
    private void Start()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.StartLevelAmbient();
    }
}
