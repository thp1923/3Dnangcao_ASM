using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    [Header("Audio Settings")]
    public AudioMixer audioMixer;
    public string exposedMusicParam = "MusicVolume";
    public float musicFadeDuration = 0.5f;
    public float musicVolumeMin = -80f;
    public float musicVolumeMax = 0f;

    [Header("Transition Canvas (should be disabled by default)")]
    public Canvas transitionCanvas;

    [Header("UI Elements")]
    public Image fadeImage;
    public Image loadingSpinner;
    public Image logoImage;

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

    #region Scene Transition
    public void FadeToScene(string sceneName)
    {
        if (!isFading)
            StartCoroutine(FadeAndLoad(sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        isFading = true;

        if (transitionCanvas != null)
            transitionCanvas.gameObject.SetActive(true);

        fadeImage.gameObject.SetActive(true);
        logoImage.enabled = true;
        loadingSpinner.enabled = true;
        if (spinnerAnimator != null)
            spinnerAnimator.enabled = true;

        // Fade out music and pause game
        StartCoroutine(FadeAudio(musicVolumeMin));
        SetGamePaused(true);

        yield return StartCoroutine(FadeMultiple(1f));

        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName);
        loadOp.allowSceneActivation = false;

        while (loadOp.progress < 0.9f)
            yield return null;

        loadOp.allowSceneActivation = true;
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

        // Fade music back in and resume game
        StartCoroutine(FadeAudio(musicVolumeMax));
        SetGamePaused(false);

        yield return StartCoroutine(FadeMultiple(0f));

        fadeImage.gameObject.SetActive(false);
        logoImage.enabled = false;
        loadingSpinner.enabled = false;
        if (spinnerAnimator != null)
            spinnerAnimator.enabled = false;

        if (transitionCanvas != null)
            transitionCanvas.gameObject.SetActive(false);

        isFading = false;
    }

    private IEnumerator FadeMultiple(float targetAlpha)
    {
        float time = 0f;

        Color fadeStart = fadeImage.color;
        Color logoStart = logoImage.color;

        float fadeFrom = fadeStart.a;
        float logoFrom = logoStart.a;

        float duration = Mathf.Max(fadeDuration, logoFadeDuration);

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float t = time / duration;

            float fadeAlpha = Mathf.Lerp(fadeFrom, targetAlpha, t);
            float logoAlpha = Mathf.Lerp(logoFrom, targetAlpha, t);

            SetAlpha(fadeImage, fadeAlpha);
            SetAlpha(logoImage, logoAlpha);

            yield return null;
        }

        SetAlpha(fadeImage, targetAlpha);
        SetAlpha(logoImage, targetAlpha);
    }
    #endregion

    #region Audio Fading
    private IEnumerator FadeAudio(float targetVolume)
    {
        if (audioMixer == null || string.IsNullOrEmpty(exposedMusicParam))
            yield break;

        audioMixer.GetFloat(exposedMusicParam, out float currentVolume);

        float time = 0f;
        while (time < musicFadeDuration)
        {
            time += Time.unscaledDeltaTime;
            float t = time / musicFadeDuration;
            float newVolume = Mathf.Lerp(currentVolume, targetVolume, t);
            audioMixer.SetFloat(exposedMusicParam, newVolume);
            yield return null;
        }

        audioMixer.SetFloat(exposedMusicParam, targetVolume);
    }
    #endregion

    private void SetGamePaused(bool paused)
    {
        Time.timeScale = paused ? 0f : 1f;
        AudioListener.pause = paused;
    }

    private void SetAlpha(Image image, float alpha)
    {
        Color col = image.color;
        col.a = alpha;
        image.color = col;
    }
}
