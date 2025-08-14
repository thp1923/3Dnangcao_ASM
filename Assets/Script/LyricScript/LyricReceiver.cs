using UnityEngine;
using System.Collections;
using UnityEngine.Playables;
using TMPro;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

[RequireComponent(typeof(PlayableDirector))]
public class LyricReceiver : MonoBehaviour, INotificationReceiver
{
    [Header("UI (TMPUGUI hoặc TextMeshPro)")]
    public TMP_Text targetTMP;

    [Header("Mặc định nếu marker không override")]
    [Range(0f, 2f)] public float defaultTypewriterSpeed = 40f; // ký tự/giây
    public bool defaultTypewriter = true;

    [Tooltip("Nếu bật, sẽ clear ngay trước khi hiển thị dòng kế tiếp.")]
    public bool clearOnNextLine = false;

    [Header("Debug")]
    public bool debugLogs = false;

    Coroutine _typing;

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        //==================== Kiểm tra TMP và notification ====================
        if (targetTMP == null) return;
        if (notification is not LyricMarker marker) return;
        //======================================================================

        // Nếu muốn xóa trước khi hiển thị câu mới
        if (clearOnNextLine) Clear(immediate: true);

        // --- Kiểm tra LocalizedString rỗng theo cách tương thích nhiều version ---
        bool tableMissing =
            marker.localizedText == null ||
            (string.IsNullOrEmpty(marker.localizedText.TableReference.TableCollectionName) &&
             marker.localizedText.TableReference.TableCollectionNameGuid == System.Guid.Empty);

        bool entryMissing =
            (string.IsNullOrEmpty(marker.localizedText.TableEntryReference.Key) &&
             marker.localizedText.TableEntryReference.KeyId == 0);

        if (tableMissing || entryMissing)
        {
            // Marker "trống" -> clear ngay, không gọi GetLocalizedStringAsync để tránh throw
            if (debugLogs) Debug.Log("[LyricReceiver] Empty marker -> CLEAR");
            Clear();
            return;
        }
        // ----------------------------------------------------------------------

        var handle = marker.localizedText.GetLocalizedStringAsync();
        handle.Completed += (AsyncOperationHandle<string> op) =>
        {
            if (op.Status != UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                return;

            string line = op.Result ?? "";

            // Nếu chuỗi trả về rỗng / chỉ khoảng trắng -> clear luôn
            if (string.IsNullOrWhiteSpace(line))
            {
                if (debugLogs) Debug.Log("[LyricReceiver] Empty string -> CLEAR");
                Clear();
                return;
            }

            // Dừng typing cũ (nếu có)
            if (_typing != null) StopCoroutine(_typing);

            bool typewriter = marker.style?.typewriter ?? defaultTypewriter;
            float cps = marker.style?.typewriterspeed ?? defaultTypewriterSpeed;

            // Dùng đúng tham số cho coroutine 4-arg của bạn
            bool clearAfter = marker.style != null && marker.style.clearOnEnd;
            float hold = Mathf.Max(0f, marker.suggestedDuration);

            if (typewriter && cps > 0f)
            {
                _typing = StartCoroutine(TypeLine(line, cps, hold, clearAfter));
            }
            else
            {
                _typing = StartCoroutine(ShowInstant(line, hold, clearAfter));
            }
        };
    }

    #region Text Settings
    IEnumerator TypeLine(string line, float charsPerSecond, float holdSeconds, bool clearAfter)
    {
        targetTMP.enabled = true;
        targetTMP.richText = true;
        targetTMP.text = line;
        targetTMP.ForceMeshUpdate();

        int total = targetTMP.textInfo.characterCount;
        if (total <= 0) total = line.Length;

        targetTMP.maxVisibleCharacters = 0;

        float visible = 0f;
        while (visible < total)
        {
            visible += charsPerSecond * Time.unscaledDeltaTime;
            targetTMP.maxVisibleCharacters = Mathf.Clamp((int)visible, 0, total);
            yield return null;
        }

        targetTMP.maxVisibleCharacters = total;

        if (clearAfter)
        {
            if (debugLogs) Debug.Log($"[LyricReceiver] Hold {holdSeconds:F2}s then CLEAR");
            if (holdSeconds > 0f)
                yield return new WaitForSecondsRealtime(holdSeconds);

            Clear(immediate: true);
        }

        _typing = null;
    }

    IEnumerator ShowInstant(string line, float holdSeconds, bool clearAfter)
    {
        targetTMP.enabled = true;
        targetTMP.richText = true;
        targetTMP.text = line;
        targetTMP.ForceMeshUpdate();
        targetTMP.maxVisibleCharacters = int.MaxValue;

        if (clearAfter)
        {
            if (debugLogs) Debug.Log($"[LyricReceiver] Hold {holdSeconds:F2}s then CLEAR");
            if (holdSeconds > 0f)
                yield return new WaitForSecondsRealtime(holdSeconds);

            Clear(immediate: true);
        }

        _typing = null;
    }

    public void Clear(bool immediate = false)
    {
        if (_typing != null) StopCoroutine(_typing);
        _typing = null;

        if (targetTMP != null)
        {
            targetTMP.text = "";
            targetTMP.maxVisibleCharacters = 0;
            if (immediate && debugLogs) Debug.Log("[LyricReceiver] CLEARED");
        }
    }
    #endregion
}
