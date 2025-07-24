using UnityEngine;
using TMPro;
using System.Collections;
public class UIDoorMes : MonoBehaviour
{
    public static UIDoorMes instance;

    public TMP_Text interactText;

    private Coroutine fadeCoroutine;

    void Awake()
    {
        instance = this;
        if (interactText != null)
        {
            SetAlpha(0);
            interactText.gameObject.SetActive(false);
        }
    }

    public void ShowMessage(string message)
    {
        if (interactText == null) return;

        interactText.text = message;
        interactText.gameObject.SetActive(true);

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeText(0f, 1f, 0.33f));
    }

    public void HideMessage()
    {
        if (interactText == null) return;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeText(1f, 0f, 0.33f, () =>
        {
            interactText.gameObject.SetActive(false);
        }));
    }

    private void SetAlpha(float alpha)
    {
        var color = interactText.color;
        color.a = alpha;
        interactText.color = color;
    }

    private IEnumerator FadeText(float from, float to, float duration, System.Action onComplete = null)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, time / duration);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(to);
        onComplete?.Invoke();
    }
}
