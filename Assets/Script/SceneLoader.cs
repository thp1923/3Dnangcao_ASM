using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; } // Singleton instance

    public GameObject loadingScreen;
    public Slider progressBar;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep SceneLoader alive
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }

        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false); // Hide loading screen initially
        }
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

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

        yield return new WaitForSeconds(0.5f); // Short delay after scene load
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }
}
