using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    [Header("Transition Canvas (should be disabled by default)")]
    public Canvas transitionCanvas;

    [Header("UI Elements")]
    public Image fadeImage;              // Fullscreen black image
    public Image loadingSpinner;         // Spinner image with optional Animator
    public Image logoImage;              // Logo to fade in/out

    [Header("Fade Settings")]
    public float fadeDuration = 0.5f;
    public float logoFadeDuration = 0.5f;

    private bool isFading = false;
    private Animator spinnerAnimator;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        spinnerAnimator = loadingSpinner.GetComponent<Animator>();

        if (transitionCanvas != null)
            transitionCanvas.gameObject.SetActive(false);

        SetAlpha(fadeImage, 0f);
        SetAlpha(logoImage, 0f);

        fadeImage.gameObject.SetActive(false);
        loadingSpinner.enabled = false;
        logoImage.enabled = false;

        if (spinnerAnimator != null)
            spinnerAnimator.enabled = false;
    }

    public void FadeToScene(string sceneName)
    {
        if (!isFading)
            StartCoroutine(FadeAndLoad(sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        isFading = true;

        // Show the canvas
        if (transitionCanvas != null)
            transitionCanvas.gameObject.SetActive(true);

        fadeImage.gameObject.SetActive(true);
        loadingSpinner.enabled = true;
        logoImage.enabled = true;

        if (spinnerAnimator != null)
            spinnerAnimator.enabled = true;

        // Fade logo in while fading to black
        yield return StartCoroutine(Fade(1f, fadeImage));
        yield return StartCoroutine(Fade(1f, logoImage));

        // Begin async loading
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName);
        loadOp.allowSceneActivation = false;

        while (loadOp.progress < 0.9f)
            yield return null;

        // Scene is ready — activate it
        loadOp.allowSceneActivation = true;

        // Scene will finish loading in background
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        StartCoroutine(FadeInAfterSceneLoad());
    }

    private IEnumerator FadeInAfterSceneLoad()
    {
        yield return null;

        // Fade both black and logo out
        yield return StartCoroutine(Fade(0f, fadeImage));
        yield return StartCoroutine(Fade(0f, logoImage));

        fadeImage.gameObject.SetActive(false);
        logoImage.enabled = false;
        loadingSpinner.enabled = false;

        if (spinnerAnimator != null)
            spinnerAnimator.enabled = false;

        if (transitionCanvas != null)
            transitionCanvas.gameObject.SetActive(false);

        isFading = false;
    }

    private IEnumerator Fade(float targetAlpha, Image image)
    {
        Color col = image.color;
        float startAlpha = col.a;
        float duration = (image == logoImage) ? logoFadeDuration : fadeDuration;
        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            col.a = alpha;
            image.color = col;
            yield return null;
        }

        col.a = targetAlpha;
        image.color = col;
    }

    private void SetAlpha(Image image, float alpha)
    {
        Color col = image.color;
        col.a = alpha;
        image.color = col;
    }
}
