using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingController : MonoBehaviour
{
    public Image progressBarFill;
    public CanvasGroup fadeGroup;   // nếu cần fade in/out
    public float fakeSpeed = 0.5f;

    private int targetSceneIndex;

    private void Start()
    {
        // Lấy index scene cần load
        targetSceneIndex = PlayerPrefs.GetInt("NextSceneIndex", 1);
        StartCoroutine(LoadAsyncScene());
        Debug.Log(">>> LoadingController START, targetSceneIndex = " + targetSceneIndex);

    }

    private IEnumerator LoadAsyncScene()
    {
        // Fade in (nếu có)
        // yield return StartCoroutine(Fade(0f, 1f, 2f));

        AsyncOperation op = SceneManager.LoadSceneAsync(targetSceneIndex);
        op.allowSceneActivation = false;

        float displayed = 0f;
        while (op.progress < 0.9f)
        {
            float realProg = op.progress / 0.9f;
            displayed = Mathf.MoveTowards(displayed, realProg, fakeSpeed * Time.deltaTime);

            progressBarFill.fillAmount = displayed;
            Debug.Log($"op.progress = {op.progress:F2}, displayed = {displayed:F2}");
            yield return null;
        }

        // Hoàn thiện thanh
        while (displayed < 1f)
        {
            displayed = Mathf.MoveTowards(displayed, 1f, fakeSpeed * Time.deltaTime);
            progressBarFill.fillAmount = displayed;
            yield return null;
        }

        // Delay nhỏ cho smooth
        yield return new WaitForSeconds(0.3f);

        // Fade out (nếu có)
        // yield return StartCoroutine(Fade(1f, 0f, 2f));

        op.allowSceneActivation = true;
    }

    private IEnumerator Fade(float from, float to, float speed)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            fadeGroup.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }
    }
}
