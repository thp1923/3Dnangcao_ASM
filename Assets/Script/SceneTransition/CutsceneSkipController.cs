using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using TMPro;
public class CutsceneSkipController : MonoBehaviour
{
    public PlayableDirector playableDirector;

    [Header("Skip Settings")]
    public float holdDuration = 2f;
    public Image holdProgressImage;
    public Image spacebarImage;
    public TMP_Text hintText;

    [Header("Fade Settings")]
    public float fadeInDuration = 0.25f;
    public float fadeOutDuration = 0.25f;
    public float idleFadeOutDelay = 0.5f;

    private float holdTimer = 0f;
    private bool isSkipping = false;
    private bool isUIVisible = false;

    private CanvasGroup canvasGroup;
    private Coroutine currentFadeRoutine;

    private float noInputTimer = 0f;

    void Start()
    {
        if (holdProgressImage != null)
            holdProgressImage.fillAmount = 0f;

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    void Update()
    {
        HandleSkipInput();
        HandleUIVisibility();
    }

    private void HandleSkipInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            holdTimer += Time.unscaledDeltaTime;
            if (holdProgressImage != null)
                holdProgressImage.fillAmount = Mathf.Clamp01(holdTimer / holdDuration);

            if (holdTimer >= holdDuration && !isSkipping)
                SkipCutscene();
        }
        else
        {
            holdTimer = 0f;
            if (holdProgressImage != null)
                holdProgressImage.fillAmount = 0f;
        }
    }

    private void HandleUIVisibility()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            noInputTimer = 0f;
            if (!isUIVisible)
                FadeUI(true);
            return;
        }

        if (Input.anyKeyDown)
        {
            noInputTimer = 0f;
            if (!isUIVisible)
                FadeUI(true);
        }
        else
        {
            noInputTimer += Time.unscaledDeltaTime;
            if (isUIVisible && noInputTimer >= idleFadeOutDelay)
                FadeUI(false);
        }
    }

    private void FadeUI(bool fadeIn)
    {
        if (currentFadeRoutine != null)
            StopCoroutine(currentFadeRoutine);

        isUIVisible = fadeIn;
        currentFadeRoutine = StartCoroutine(FadeUICoroutine(fadeIn));
    }

    private IEnumerator FadeUICoroutine(bool fadeIn)
    {
        float duration = fadeIn ? fadeInDuration : fadeOutDuration;
        float targetAlpha = fadeIn ? 1f : 0f;

        float time = 0f;
        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float step = Time.unscaledDeltaTime / duration;
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, step);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
        currentFadeRoutine = null;
    }

    public void SkipCutscene()
    {
        if (isSkipping) return;
        isSkipping = true;

        if (playableDirector != null)
        {
            playableDirector.time = playableDirector.duration;
            playableDirector.Evaluate();  // Đảm bảo timeline "nhảy" đến cuối
            playableDirector.Stop();      // Dừng lại để không tiếp tục chạy
        }

        // ✳️ Không load scene nữa — để script khác tiếp tục logic sau cutscene
        //Debug.Log("Cutscene skipped. Waiting for other scripts to continue...");
    }
}
