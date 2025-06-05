using TMPro;
using UnityEngine;

public class LoadingTextAnimator : MonoBehaviour
{
    public TextMeshProUGUI loadingText;
    public float interval = 0.5f;

    private string baseText = "Loading";
    private float timer;
    private int dotCount = 0;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0f;
            dotCount = (dotCount + 1) % 4; // 0 → 3

            loadingText.text = baseText + new string('.', dotCount);
        }
    }
}
