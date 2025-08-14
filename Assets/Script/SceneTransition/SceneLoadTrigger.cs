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

        hasTriggered = true; // chống đúp

        var gsm = GameAutoSaveManager.Instance;
        if (gsm != null)
        {
            // báo cho GSM biết sắp đổi scene
            gsm.PrepareSceneChange();

            // lưu inventory trước
            InventoryManager.Instance?.SaveInventoryForSlot(gsm.saveSlot);

            // save state rồi mới chuyển scene
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
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }

    private void OnDisable()
    {
        hasTriggered = false;
    }
}
