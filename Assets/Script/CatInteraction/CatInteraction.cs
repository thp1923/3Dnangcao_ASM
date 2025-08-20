using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CatInteraction : MonoBehaviour
{
    public GameJoltManager gameJoltManager;
    public TrophyType CAT = TrophyType.SecretCatTrophy;
    public TMP_Text interactText;
    public Animator catAnimator;
    public AudioSource audioSource;

    private bool playerNear = false;
    private bool hasInteracted = false;

    void Start()
    {
        SetAlpha(0);
        interactText.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            interactText.gameObject.SetActive(true);
            StartCoroutine(FadeText(0, 1, 0.33f));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            interactText.gameObject.SetActive(false);
            StartCoroutine(FadeText(1, 0, 0.33f, ()
            => interactText.gameObject.SetActive(false)));
        }
    }
    void Update()
    {
        if (playerNear && !hasInteracted && Input.GetKeyDown(KeyCode.F)
        && catAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            hasInteracted = true;
            StartCoroutine(FadeText(1, 0, 0.2f, () => interactText.gameObject.SetActive(false)));
            StartCoroutine(DoOiaCatSequence());
        }
    }
    void SetAlpha(float alpha)
    {
        var c = interactText.color;
        c.a = alpha;
        interactText.color = c;
    }
    System.Collections.IEnumerator FadeText(float from, float to, float duration, System.Action onComplete = null)
    {
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, t / duration);
            SetAlpha(alpha);
            yield return null;
        }
        SetAlpha(to);
        onComplete?.Invoke();
    }
    System.Collections.IEnumerator DoOiaCatSequence()
    {
        catAnimator.SetTrigger("Spin");

        // Restart audio
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.Play();
        }

        // Wait until the animator transitions to the "Spin" state
        yield return new WaitUntil(() => catAnimator.GetCurrentAnimatorStateInfo(0).IsName("Spin"));

        // Now we can safely get the animation length
        float animLength = catAnimator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(animLength);

        // Stop audio after the animation ends
        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();

        yield return new WaitForSeconds(1f);

        if (gameJoltManager != null)
            gameJoltManager.UnlockTrophy(CAT);

        hasInteracted = false;
    }
}
