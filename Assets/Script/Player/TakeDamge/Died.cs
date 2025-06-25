using UnityEngine.SceneManagement;
using UnityEngine;

public class Died : MonoBehaviour
{
    public void ResetScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(currentScene);
    }
}
