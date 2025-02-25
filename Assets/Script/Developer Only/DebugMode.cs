using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class DebugMode : MonoBehaviour
{
    public bool IsDebugMode { get; private set; } = false;
    public GameObject debugPanel; // Assign Debug UI Panel in Inspector
    public GameObject loadingPanel; // Assign your Loading UI Panel in Inspector
    public Slider progressBar; // Assign the UI Slider in Inspector

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) // Press F1 to toggle Debug Mode
        {
            ToggleDebugMode();
        }

        if (IsDebugMode && Input.GetKeyDown(KeyCode.F2)) // Press F2 to load the next scene
        {
            StartCoroutine(LoadNextScene());
        }
    }

    void ToggleDebugMode()
    {
        IsDebugMode = !IsDebugMode;
        Debug.Log("Debug Mode: " + (IsDebugMode ? "Enabled" : "Disabled"));

        if (debugPanel != null)
        {
            debugPanel.SetActive(IsDebugMode); // Show/Hide Debug UI
        }
    }

    IEnumerator LoadNextScene()
    {
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(true); // Show loading panel
        }

        if (progressBar != null)
        {
            progressBar.value = 0f; // Reset progress bar
        }

        yield return new WaitForSeconds(0.5f); // Small delay to show UI

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneIndex);
            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                float targetProgress = asyncLoad.progress < 0.9f ? asyncLoad.progress : 1f; // Limit progress
                if (progressBar != null)
                {
                    // Smooth animation for the progress bar
                    while (progressBar.value < targetProgress)
                    {
                        progressBar.value += Time.deltaTime * 0.5f; // Adjust speed as needed
                        yield return null;
                    }
                }

                if (asyncLoad.progress >= 0.9f)
                {
                    yield return new WaitForSeconds(0.5f); // Optional delay
                    asyncLoad.allowSceneActivation = true; // Load the scene
                }

                yield return null;
            }
        }
        else
        {
            Debug.LogWarning("No next scene found in Build Settings!");
            if (loadingPanel != null)
            {
                loadingPanel.SetActive(false); // Hide loading panel if no next scene
            }
        }
    }
}