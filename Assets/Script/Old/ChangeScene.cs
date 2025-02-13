using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public CanvasGroup playButton, exitButton, optionsButton, gameTitle, optionsPanel;
    private bool isOptionsOpen = false;

    void Start()
    {
        ResetUI();
    }

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

    IEnumerator FadeOutUIAndLoadScene()
    {
        yield return StartCoroutine(FadeOutAllUI(0.5f));
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Loading next scene...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator FadeOutUIAndOpenOptions()
    {
        isOptionsOpen = true;
        yield return StartCoroutine(FadeOutAllUI(0.5f));
        yield return new WaitForSeconds(1f);
        optionsPanel.gameObject.SetActive(true);
        yield return StartCoroutine(FadeInUI(optionsPanel, 0.5f));
    }

    IEnumerator FadeOutUIAndQuit()
    {
        yield return StartCoroutine(FadeOutAllUI(0.5f));
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    // ✅ This fades out all UI elements at the SAME time
    IEnumerator FadeOutAllUI(float duration)
    {
        float time = 0;
        float startAlpha = gameTitle.alpha; // All elements start at the same alpha

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

        // Disable interaction after fade-out
        playButton.interactable = false;
        optionsButton.interactable = false;
        exitButton.interactable = false;

        playButton.blocksRaycasts = false;
        optionsButton.blocksRaycasts = false;
        exitButton.blocksRaycasts = false;
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
}
