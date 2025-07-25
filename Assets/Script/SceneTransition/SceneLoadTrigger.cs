using UnityEngine;

public class SceneLoadTrigger : MonoBehaviour
{
    [Header("Scene Load Settings")]
    public string sceneName;
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider col)
    {
        if (hasTriggered) return;

        if (col.CompareTag("Player") && !string.IsNullOrEmpty(sceneName))
        {
            hasTriggered = true;
            SceneTransitionManager.Instance.FadeToScene(sceneName);
        }
    }
}
