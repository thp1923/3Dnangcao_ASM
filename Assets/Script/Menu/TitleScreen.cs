using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class TitleScreenManager : MonoBehaviour
{
    public TextMeshProUGUI pressAnyButtonText;
    public GameObject mainMenuPanel;
    public float fadeDuration = 1.5f;

    private bool hasPressed = false;

    void Start()
    {
        mainMenuPanel.SetActive(false); // Ẩn menu lúc đầu
        StartCoroutine(FadeLoop());
    }

    void Update()
    {
        if (!hasPressed && Input.anyKeyDown)
        {
            hasPressed = true;
            StopAllCoroutines();
            StartCoroutine(ShowMainMenu());
        }
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

    IEnumerator ShowMainMenu()
    {
        // Ẩn chữ "Press Any Button" bằng fade out
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
        mainMenuPanel.SetActive(true);
    }
}
