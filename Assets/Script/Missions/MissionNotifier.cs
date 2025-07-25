using UnityEngine;
using TMPro;
using System.Collections;
public class MissionNotifier : MonoBehaviour
{
    public TMP_Text notificationText;
    public float fadeDuration = 0.5f;
    public float displayDuration = 2f;

    private Coroutine currentCoroutine;

    public void ShowMissionNotification(string message)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(ShowNotificationCoroutine(message));
    }

    private IEnumerator ShowNotificationCoroutine(string message)
    {
        notificationText.text = message;
        notificationText.gameObject.SetActive(true);

        yield return StartCoroutine(FadeTextAlpha(0f, 1f));

        yield return new WaitForSeconds(displayDuration);

        yield return StartCoroutine(FadeTextAlpha(1f, 0f));

        notificationText.gameObject.SetActive(false);

    }

    private IEnumerator FadeTextAlpha(float from, float to)
    {
        float elapsed = 0f;
        Color color = notificationText.color;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            color.a = alpha;
            notificationText.color = new Color(color.r, color.g, color.b, to);
            yield return null;
        }
        notificationText.color = new Color(color.r, color.g, color.b, to);
    }
}
