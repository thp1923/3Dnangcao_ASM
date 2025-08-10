using UnityEngine;

public class SceneLoadTrigger : MonoBehaviour
{
    [Header("Scene Load Settings")]
    public string sceneName;
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider col)
    {
        if (hasTriggered) return;
        if (!col.CompareTag("Player")) return;
        if (string.IsNullOrEmpty(sceneName)) return;

        hasTriggered = true; // chống đúp ngay khi đủ điều kiện

        var gsm = GameAutoSaveManager.Instance;
        if (gsm != null)
        {
            // báo cho GSM biết sắp đổi scene (để áp snapshot RAM khi sang map mới)
            gsm.PrepareSceneChange();

            // save rồi mới chuyển scene (an toàn)
            gsm.SaveCurrentGame(() =>
            {
                if (SceneTransitionManager.Instance != null)
                    SceneTransitionManager.Instance.FadeToScene(sceneName);
                else
                    UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            });
        }
        else
        {
            // fallback nếu GSM không có (không khuyến khích)
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }

    // Nếu object bị disable/enable lại, cho phép kích lại trigger
    private void OnDisable()
    {
        hasTriggered = false;
    }
}
