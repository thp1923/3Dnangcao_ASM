using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    #region Attributes
    public CanvasGroup playButton, exitButton, optionsButton, gameTitle, optionsPanel;
    public GameObject loadingScreen;
    public Slider progressBar;

    private bool isOptionsOpen = false;
    #endregion
    void Start()
    {
        ResetUI();
    }
    #region UI
    void ResetUI()
    {
        playButton.alpha = 1;
        optionsButton.alpha = 1;
        exitButton.alpha = 1;
        gameTitle.alpha = 1;

        playButton.interactable = true;
        optionsButton.interactable = true;
        exitButton.interactable = true;

        playButton.blocksRaycasts = true;
        optionsButton.blocksRaycasts = true;
        exitButton.blocksRaycasts = true;

        optionsPanel.alpha = 0;
        optionsPanel.gameObject.SetActive(false);

        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }

    public void NextScene()
    {
        StartCoroutine(FadeOutUIAndLoadScene());
    }

    public void Quit()
    {
        StartCoroutine(FadeOutUIAndQuit());
    }

    public void OpenOptions()
    {
        if (!isOptionsOpen)
        {
            StartCoroutine(FadeOutUIAndOpenOptions());
        }
    }

    public void CloseOptions()
    {
        if (isOptionsOpen)
        {
            StartCoroutine(FadeOutOptionsAndRestoreUI());
        }
    }
    #endregion
    #region Features
    IEnumerator FadeOutUIAndLoadScene()
    {
        yield return StartCoroutine(FadeOutAllUI(0.5f));
        yield return new WaitForSeconds(0.5f);

        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }
        yield return StartCoroutine(LoadSceneAsync());
    }
    #region Scene Loading
    IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        operation.allowSceneActivation = false; // Prevent auto-switching

        float targetProgress = 0;

        while (!operation.isDone)
        {
            // Normalize progress (0 to 1)
            targetProgress = Mathf.Clamp01(operation.progress / 0.9f);

            // Smoothly update the slider over time
            if (progressBar != null)
            {
                progressBar.value = Mathf.MoveTowards(progressBar.value, targetProgress, Time.deltaTime * 2f);
            }

            // Wait until the scene is fully loaded (progress = 0.9)
            if (operation.progress >= 0.9f)
            {
                // Force the slider to complete the animation to 100%
                while (progressBar.value < 1f)
                {
                    progressBar.value = Mathf.MoveTowards(progressBar.value, 1f, Time.deltaTime * 2f);
                    yield return null;
                }

                yield return new WaitForSeconds(0.5f); // Optional delay for smooth transition
                operation.allowSceneActivation = true; // Now switch scenes
            }

            yield return null;
        }
    }
    #endregion
    IEnumerator FadeOutUIAndOpenOptions()
    {
        isOptionsOpen = true;
        yield return StartCoroutine(FadeOutAllUI(0.5f));
        yield return new WaitForSeconds(1f);
        optionsPanel.gameObject.SetActive(true);
        yield return StartCoroutine(FadeInUI(optionsPanel, 0.5f));
    }

    IEnumerator FadeOutOptionsAndRestoreUI()
    {
        yield return StartCoroutine(FadeOutUI(optionsPanel, 0.5f));
        optionsPanel.gameObject.SetActive(false);
        yield return StartCoroutine(FadeInAllUI(0.5f));
        isOptionsOpen = false;
    }

    IEnumerator FadeOutUIAndQuit()
    {
        yield return StartCoroutine(FadeOutAllUI(0.5f));
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }

    IEnumerator FadeOutAllUI(float duration)
    {
        float time = 0;
        float startAlpha = gameTitle.alpha;

        while (time < duration)
        {
            time += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 0, time / duration);

            playButton.alpha = newAlpha;
            optionsButton.alpha = newAlpha;
            exitButton.alpha = newAlpha;
            gameTitle.alpha = newAlpha;

            yield return null;
        }

        playButton.interactable = false;
        optionsButton.interactable = false;
        exitButton.interactable = false;

        playButton.blocksRaycasts = false;
        optionsButton.blocksRaycasts = false;
        exitButton.blocksRaycasts = false;
    }

    IEnumerator FadeInAllUI(float duration)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float newAlpha = Mathf.Lerp(0, 1, time / duration);

            playButton.alpha = newAlpha;
            optionsButton.alpha = newAlpha;
            exitButton.alpha = newAlpha;
            gameTitle.alpha = newAlpha;

            yield return null;
        }

        playButton.interactable = true;
        optionsButton.interactable = true;
        exitButton.interactable = true;

        playButton.blocksRaycasts = true;
        optionsButton.blocksRaycasts = true;
        exitButton.blocksRaycasts = true;
    }

    IEnumerator FadeOutUI(CanvasGroup uiElement, float duration)
    {
        float time = 0;
        float startAlpha = uiElement.alpha;
        while (time < duration)
        {
            time += Time.deltaTime;
            uiElement.alpha = Mathf.Lerp(startAlpha, 0, time / duration);
            yield return null;
        }
        uiElement.alpha = 0;
    }

    IEnumerator FadeInUI(CanvasGroup uiElement, float duration)
    {
        float time = 0;
        uiElement.alpha = 0;
        uiElement.interactable = true;
        uiElement.blocksRaycasts = true;

        while (time < duration)
        {
            time += Time.deltaTime;
            uiElement.alpha = Mathf.Lerp(0, 1, time / duration);
            yield return null;
        }

        uiElement.alpha = 1;
    }
    #endregion
}
