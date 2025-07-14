using UnityEngine;
using TMPro;
using System.Collections;
public class MissionNotificationUI : MonoBehaviour
{
    public CanvasGroup NotificationGroup;
    public TMP_Text missionText;
    public float fadeDuration = 1f;
    public float displayDuration = 2f;
    public void ShowMission(string message)
    {
        StopAllCoroutines();
        StartCoroutine(FadeNotification(message));
    }
    private IEnumerator FadeNotification(string message)
    {
        missionText.text = message;

        for (float t = 0; t < 1; t += Time.deltaTime / fadeDuration)
        {
            NotificationGroup.alpha = t;
            yield return null;
        }

        NotificationGroup.alpha = 1;
        yield return new WaitForSeconds(displayDuration);

        for (float t = 1; t > 0; t -= Time.deltaTime / fadeDuration)
        {
            NotificationGroup.alpha = t;
            yield return null;
        }

        NotificationGroup.alpha = 0;
    }
}
