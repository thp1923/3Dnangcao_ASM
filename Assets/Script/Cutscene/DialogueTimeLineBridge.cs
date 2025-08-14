using DialogueEditor;
using UnityEngine;

public class DialogueTimeLineBridge : MonoBehaviour
{
    public TimeLineControl timelineControl;

    private void OnEnable()
    {
        ConversationManager.OnConversationEnded += ResumeTimelineFromDialogue;
    }

    private void OnDisable()
    {
        ConversationManager.OnConversationEnded -= ResumeTimelineFromDialogue;
    }

    private void ResumeTimelineFromDialogue()
    {
        if (timelineControl != null)
        {
            timelineControl.ResumeTimeline();
        }
    }
}
