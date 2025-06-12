using UnityEngine;

public class AreaLoader : MonoBehaviour
{
    public void LoadNextScene()
    {
        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
        {
            SceneLoader.Instance.LoadScene(nextSceneIndex);
        }
    }
}