using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadGameFromCutScene : MonoBehaviour
{
    public GameObject loadingScreen; // Reference to UI
    public Slider progressBar; // Optional progress bar
    // Start is called before the first frame update
    void Start()
    {
        LoadNextScene();
    }
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(LoadSceneAsync(nextSceneIndex));
        }
    }

    IEnumerator LoadSceneAsync(int sceneIndex)
    {
        loadingScreen.SetActive(true); // Show loading screen

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false; // Wait until fully loaded

        while (operation.progress < 0.9f)
        {
            if (progressBar != null)
                progressBar.value = operation.progress; // Update UI

            yield return null;
        }

        yield return new WaitForSeconds(1f); // Optional delay for smooth transition

        operation.allowSceneActivation = true; // Load the scene
    }
}
