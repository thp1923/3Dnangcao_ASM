using UnityEngine;
using System.Collections;
using UnityEngine.Playables;
using TMPro;
using UnityEngine.ResourceManagement.AsyncOperations;

[RequireComponent(typeof(PlayableDirector))]
public class LyricReceiver : MonoBehaviour, INotificationReceiver
{
    [Header("UI (TMPUGUI)")]
    public TMP_Text targetTMP;

    [Header("Default Style")]
    [Range(0f, 2f)] public float defaultTypewriterSpeed = 40f; // ký tự/giây
    public bool defaultTypewriter = true;
    public bool clearOnNextLine = true;

    Coroutine _typing;
    public void OnNotify(Playable origin, INotification notification, object context)
    {
        //==================== Kiểm tra TMP và notification ====================
        if (targetTMP == null) return;
        if (notification is not LyricMarker marker) return;
        //======================================================================
        var handle = marker.localizedText.GetLocalizedStringAsync();
        handle.Completed += (AsyncOperationHandle<string> op) =>
        {
            if (op.Status != UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                return;
            string line = op.Result ?? "";
            if (_typing != null) StopCoroutine(_typing); //Dừng việc gõ chữ

            bool typewriter = marker.style?.typewriter ?? defaultTypewriter;
            float cps = marker.style?.typewriterspeed ?? defaultTypewriterSpeed;

            if (typewriter && cps > 0f)
                _typing = StartCoroutine(TypeLine(line, cps, marker.suggestedDuration));
            else
                ShowInstant(line);
        };

    }

    #region Text Settings
    IEnumerator TypeLine(string line, float charPerSecond, float suggestedDuration)
    {
        targetTMP.richText = true; // Bật rich text để hỗ trợ định dạng màu chữ
        targetTMP.text = line;
        targetTMP.ForceMeshUpdate(); // Cập nhật mesh để hiển thị chữ kể cả khi rich text
        int total = targetTMP.textInfo.characterCount;

        targetTMP.maxVisibleCharacters = 0;

        float visible = 0f;
        while (visible < total)
        {
            visible += charPerSecond * Time.unscaledDeltaTime;
            targetTMP.maxVisibleCharacters = Mathf.Clamp((int)visible, 0, total);
            yield return null;
        }
        targetTMP.maxVisibleCharacters = total;

        if (suggestedDuration > 0f)
            yield return new WaitForSecondsRealtime(Mathf.Max(0f, suggestedDuration * 0.25f));

        _typing = null;
    }

    void ShowInstant(string line)
    {
        targetTMP.richText = true;
        targetTMP.text = line;
        targetTMP.ForceMeshUpdate();
        targetTMP.maxVisibleCharacters = int.MaxValue;
    }

    public void Clear()
    {
        if (_typing != null) StopCoroutine(_typing);
        _typing = null;
        if (targetTMP != null)
        {
            targetTMP.text = "";
            targetTMP.maxVisibleCharacters = 0;
        }
    }
    #endregion
}
