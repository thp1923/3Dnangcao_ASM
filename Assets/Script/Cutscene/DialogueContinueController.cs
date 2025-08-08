using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueContinueController : MonoBehaviour
{
    [Header("Tham chiếu tới TimelineControl")]
    public TimeLineControl timelineControl;

    private ConversationManager convManager;

    private void Start()
    {
        convManager = ConversationManager.Instance;
    }

    private void Update()
    {
        if (convManager == null || !convManager.IsConversationActive)
            return;

        // Tìm tất cả các button đang hiển thị
        var buttons = convManager.GetComponentsInChildren<Button>(true);
        foreach (var btn in buttons)
        {
            // Nếu chưa gắn sự kiện thì gắn
            if (!HasListener(btn))
            {
                btn.onClick.AddListener(OnContinueClicked);
            }
        }
    }

    private bool HasListener(Button btn)
    {
        // Kiểm tra nếu đã gắn listener của script này
        var persistentCount = btn.onClick.GetPersistentEventCount();
        for (int i = 0; i < persistentCount; i++)
        {
            if (btn.onClick.GetPersistentTarget(i) == this)
                return true;
        }
        return false;
    }

    private void OnContinueClicked()
    {
        if (timelineControl != null)
        {
            timelineControl.ResumeTimeline();
        }
    }
}
