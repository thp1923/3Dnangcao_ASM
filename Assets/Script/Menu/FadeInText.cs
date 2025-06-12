using UnityEngine;
using TMPro;
using System.Collections;

public class FadeInText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float fadeDuration = 1.5f;

    void Start()
    {
        StartCoroutine(FadeLoop());
    }

    IEnumerator FadeLoop()
    {
        Color c = text.color;

        while (true)
        {
            // Fade In
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                c.a = Mathf.Lerp(0, 1, t / fadeDuration);
                text.color = c;
                yield return null;
            }

            // Fade Out
            t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                c.a = Mathf.Lerp(1, 0, t / fadeDuration);
                text.color = c;
                yield return null;
            }
        }
    }
}
