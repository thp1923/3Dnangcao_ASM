using System.Collections;
using UnityEngine;

public class LogoFade : MonoBehaviour
{
    public CanvasGroup logoCanvasGroup;
    public float fadeDuration = 2f;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            logoCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
        logoCanvasGroup.alpha = 1f;
    }
}
