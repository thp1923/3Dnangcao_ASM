using System.Collections;
using UnityEngine;

    public class QuestPanelController : MonoBehaviour
{
    public CanvasGroup questPanelGroup;
    public float fadeDuration = 0.3f;
    private bool isOpen = false;
    private Coroutine currentRoutine;
    

    void Start()
    {
        questPanelGroup.alpha = 0f;
        questPanelGroup.interactable = false;
        questPanelGroup.blocksRaycasts = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (currentRoutine != null)
                StopCoroutine(currentRoutine);

            if (!isOpen)
            {
                currentRoutine = StartCoroutine(FadeIn());
                PlayerAttackController.CursorLocked = false;
            }
            else
            {
                currentRoutine = StartCoroutine(FadeOut());
                PlayerAttackController.CursorLocked = true;
            }
                

            isOpen = !isOpen;
        }
    }

    IEnumerator FadeIn()
    {
        float elapsed = 0f;
        questPanelGroup.gameObject.SetActive(true);
        questPanelGroup.interactable = true;
        questPanelGroup.blocksRaycasts = true;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            questPanelGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        questPanelGroup.alpha = 1f;
    }

    IEnumerator FadeOut()
    {
        float elapsed = 0f;

        questPanelGroup.interactable = false;
        questPanelGroup.blocksRaycasts = false;

        float startAlpha = questPanelGroup.alpha;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            questPanelGroup.alpha = Mathf.Clamp01(1 - (elapsed / fadeDuration));
            yield return null;
        }

        questPanelGroup.alpha = 0f;
        questPanelGroup.gameObject.SetActive(false);
    }
}
