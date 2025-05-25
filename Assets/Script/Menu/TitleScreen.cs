using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class TitleScreenManager : MonoBehaviour
{
    public TextMeshProUGUI pressAnyButtonText;
    public GameObject messagePanel;      // <- NEW
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;

    public float fadeDuration = 1.5f;

    private int pressStep = 0;

    void Start()
    {
        mainMenuPanel.SetActive(false); 
        messagePanel.SetActive(false); // <- NEW
        StartCoroutine(FadeLoop());
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            pressStep++;

            if (pressStep == 1)
            {
                StopAllCoroutines();
                StartCoroutine(FadeOutPressText());
            }
            else if (pressStep == 2)
            {
                messagePanel.SetActive(false);
                mainMenuPanel.SetActive(true);
                this.enabled = false;
            }
        }
    }
    public void ShowOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    IEnumerator FadeLoop()
    {
        Color c = pressAnyButtonText.color;

        while (true)
        {
            // Fade In
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                c.a = Mathf.Lerp(0, 1, t / fadeDuration);
                pressAnyButtonText.color = c;
                yield return null;
            }

            // Fade Out
            t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                c.a = Mathf.Lerp(1, 0, t / fadeDuration);
                pressAnyButtonText.color = c;
                yield return null;
            }
        }
    }

    IEnumerator FadeOutPressText()
    {
        Color c = pressAnyButtonText.color;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1, 0, t / fadeDuration);
            pressAnyButtonText.color = c;
            yield return null;
        }

        pressAnyButtonText.gameObject.SetActive(false);
        messagePanel.SetActive(true); // <- hiện message
    }
}
