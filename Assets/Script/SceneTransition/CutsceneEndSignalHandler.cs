using UnityEngine;

public class CutsceneEndSignalHandler : MonoBehaviour
{
    public string sceneToLoad;

    public void OnCutsceneEnded()
    {
        SceneTransitionManager.Instance.FadeToScene(sceneToLoad);
    }
}
