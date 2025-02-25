using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AreaLoader : MonoBehaviour
{
    public GameObject loadingScreen; // Reference to UI
    public Slider progressBar; // Optional progress bar

    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(LoadSceneAsync(nextSceneIndex));
        }
        else
        {
            Debug.LogWarning("No more scenes to load! You are at the last scene.");
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