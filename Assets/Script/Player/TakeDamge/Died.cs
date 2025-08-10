using UnityEngine.SceneManagement;
using UnityEngine;

public class Died : MonoBehaviour
{
    public void ResetScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        GameAutoSaveManager.Instance.OnPlayerDie();
        SceneManager.LoadSceneAsync(currentScene);
    }
}
