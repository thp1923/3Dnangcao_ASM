using UnityEngine;

public class SceneTransitionProxy : MonoBehaviour
{

    [Header("Scene to load")]
    public string nextSceneName;
    public void triggerTransition()
    {
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.FadeToScene(nextSceneName);
        }
        else
        {
            Debug.Log("SceneTransitionManager instance not found in the current scene!");
            return;
        }
    }
}
